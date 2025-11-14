using AutoMapper;
using BusinessObjects;
using DTOs;
using Repositories;

namespace Services
{
    public class CartService : ICartService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductOptionRepository _productOptionRepository;
        private readonly IMapper _mapper;

        public CartService(ICartItemRepository cartItemRepository, IProductOptionRepository productOptionRepository, IMapper mapper)
        {
            _cartItemRepository = cartItemRepository;
            _productOptionRepository = productOptionRepository;
            _mapper = mapper;
        }

        public async Task<UserCartResponseDto> GetAllCartItems(int userId)
        {
            var cartItems = await _cartItemRepository.GetAllCartItems(userId);

            return new UserCartResponseDto
            {
                UserId = userId,
                CartItems = _mapper.Map<List<CartItemResponseDto>>(cartItems)
            };
        }

        public async Task<CartItemResponseDto> GetCartItemById(int cartItemId)
        {
            var cartItem = await _cartItemRepository.GetByIdAsync(cartItemId);
            return _mapper.Map<CartItemResponseDto>(cartItem);
        }

        public async Task<CartItem> AddToCart(CartItemRequestDto dto, int userId)
        {
            var productOptions = await _productOptionRepository.GetByIdsAsync(dto.ProductOptionIds, dto.ProductId);

            var cartItem = _mapper.Map<CartItem>(dto);
            cartItem.UserId = userId;
            cartItem.ProductOptions = productOptions;

            return await _cartItemRepository.AddToCartAsync(cartItem);
        }

        public async Task UpdateQuantity(int cartItemId, int quantity)
        {
            var item = await _cartItemRepository.GetByIdAsync(cartItemId);
            item.Quantity = quantity;
            await _cartItemRepository.UpdateAsync(item);
        }

        public async Task DeleteCartItem(int cartItemId)
        {
            await _cartItemRepository.DeleteAsync(cartItemId);
        }

        public async Task ClearCart(int userId)
        {
            await _cartItemRepository.DeleteByUserIdAsync(userId);
        }

        public async Task<bool> IsCartItemExisted(int userId, int productId, List<int> optionIds)
        {
            return await _cartItemRepository.ExistsAsync(userId, productId, optionIds);
        }
    }
}