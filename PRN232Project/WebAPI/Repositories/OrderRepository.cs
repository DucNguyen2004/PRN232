using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductOptions)
                        .ThenInclude(po => po.OptionValue)
                            .ThenInclude(ov => ov.Option)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductOptions)
                        .ThenInclude(po => po.OptionValue)
                            .ThenInclude(ov => ov.Option)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductOptions)
                        .ThenInclude(po => po.OptionValue)
                            .ThenInclude(ov => ov.Option)
                .OrderByDescending(o => o.Id)
                .ToListAsync();
        }

        public async Task<Order> AddAsync(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductOptions)
                        .ThenInclude(po => po.OptionValue)
                            .ThenInclude(ov => ov.Option)
                .FirstAsync(o => o.Id == order.Id);
        }

        public async Task UpdateAsync(int id, Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));

            var tracked = await _context.Orders.FindAsync(id);
            if (tracked == null)
                throw new KeyNotFoundException($"Order with Id {id} not found.");

            order.Id = order.Id;
            _context.Entry(tracked).CurrentValues.SetValues(order);

            await _context.SaveChangesAsync();
        }
    }
}
