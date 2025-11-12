using BusinessObjects;

namespace Repositories
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<IEnumerable<Order>> GetAllByUserIdAsync(int userId);
        Task<Order> AddAsync(Order order);
        Task UpdateAsync(int id, Order order);
    }
}
