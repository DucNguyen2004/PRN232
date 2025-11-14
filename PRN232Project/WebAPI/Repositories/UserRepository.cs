using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Roles)
                .Include(u => u.Orders)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            return await _context.Users
                .Include(u => u.Roles)
                .Include(u => u.Orders)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username cannot be null or empty.", nameof(username));
            return await _context.Users
                .Include(u => u.Roles)
                .Include(u => u.Orders)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> GetByPhoneAsync(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) throw new ArgumentException("Role name cannot be null or empty.", nameof(phone));
            return await _context.Users
                .Include(u => u.Roles)
                .Include(u => u.Orders)
                .FirstOrDefaultAsync(u => u.Phone == phone);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Roles)
                .Include(u => u.Orders)
                .ToListAsync();
        }

        public async Task<(IEnumerable<User> Items, int Total)> GetPagedAsync(int page, int pageSize)
        {
            var query = _context.Users
                .Include(u => u.Roles)
                .Include(u => u.Orders)
                .AsQueryable();

            int total = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }


        public async Task<User> AddAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task UpdateAsync(int id, User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var tracked = await _context.Users.FindAsync(id);
            if (tracked == null)
                throw new KeyNotFoundException($"User with Id {id} not found.");

            user.Id = tracked.Id;
            _context.Entry(tracked).CurrentValues.SetValues(user);

            tracked.Roles.Clear();
            foreach (var role in user.Roles)
            {
                tracked.Roles.Add(role);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User with Id {id} not found.");

            user.Status = "DELETED";
            await _context.SaveChangesAsync();
        }
    }
}
