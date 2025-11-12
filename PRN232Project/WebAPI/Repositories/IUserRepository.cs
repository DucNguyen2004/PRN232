using BusinessObjects;

namespace Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByPhoneAsync(string phone);
        Task<IEnumerable<User>> GetAllAsync();
        Task<(IEnumerable<User> Items, int Total)> GetPagedAsync(int page, int pageSize);
        Task<User> AddAsync(User user);
        Task UpdateAsync(int id, User user);
        Task DeleteAsync(int id);
    }
}
