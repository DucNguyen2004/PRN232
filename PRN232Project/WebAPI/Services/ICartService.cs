using BusinessObjects;
using DTOs;

namespace Services
{
    public interface ICartService
    {
        Task<UserCartResponseDto> GetAllCartItems(int userId);
        Task<CartItemResponseDto> GetCartItemById(int cartItemId);
        Task<CartItem> AddToCart(CartItemRequestDto dto, int userId);
        Task UpdateQuantity(int cartItemId, int quantity);
        Task DeleteCartItem(int cartItemId);
        Task ClearCart(int userId);
        Task<bool> IsCartItemExisted(int userId, int productId, List<int> optionIds);
    }
}
