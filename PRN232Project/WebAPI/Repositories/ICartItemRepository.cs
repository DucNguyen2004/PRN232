using BusinessObjects;

namespace Repositories
{
    public interface ICartItemRepository
    {
        Task<IEnumerable<CartItem>> GetAllCartItems(int userId);
        Task<bool> ExistsAsync(int userId, int productId, List<int> productOptionIds);
        Task<CartItem> AddToCartAsync(CartItem item);
        Task UpdateAsync(CartItem item);
        Task DeleteAsync(int id);
        Task<CartItem?> GetByIdAsync(int id);
        Task DeleteByUserIdAsync(int userId);
    }
}
