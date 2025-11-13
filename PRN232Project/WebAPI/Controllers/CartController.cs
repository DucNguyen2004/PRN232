using AutoMapper;
using BusinessObjects;
using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Security.Claims;

namespace PRN232Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;

        public CartController(ICartService cartService, IMapper mapper)
        {
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<UserCartResponseDto>> GetCart()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return BadRequest("Invalid or missing user ID in token.");
            }

            var cart = await _cartService.GetAllCartItems(userId);

            if (cart == null)
            {
                return NotFound("No cart item found from user ID: " + userId);
            }

            return Ok(cart);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CartItemResponseDto>> GetCartItemById(int id)
        {
            var cart = await _cartService.GetCartItemById(id);
            if (cart == null)
            {
                return NotFound("Cart not found");
            }

            return Ok(cart);
        }

        [HttpPost]
        public async Task<ActionResult<CartItemResponseDto>> AddToCart([FromBody] CartItemRequestDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid cart item data.");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return BadRequest("Invalid or missing user ID in token.");
            }

            if (await _cartService.IsCartItemExisted(userId, dto.ProductId, dto.ProductOptionIds))
            {
                return BadRequest("This product with selected options is already in the cart.");
            }

            CartItem cartItem = await _cartService.AddToCart(dto, userId);
            return CreatedAtAction(nameof(GetCartItemById), new { id = cartItem.Id }, _mapper.Map<CartItemResponseDto>(cartItem));
        }

        [HttpPut("{cartItemId:int}")]
        public async Task<ActionResult> UpdateCartItemQuantity(int cartItemId, [FromBody] int newQuantity)
        {
            var cart = await _cartService.GetCartItemById(cartItemId);
            if (cart == null)
            {
                return NotFound("Cart not found");
            }

            await _cartService.UpdateQuantity(cartItemId, newQuantity);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteCartItem(int id)
        {
            var cart = await _cartService.GetCartItemById(id);
            if (cart == null)
            {
                return NotFound("Cart not found");
            }

            await _cartService.DeleteCartItem(id);
            return NoContent();
        }

        [HttpDelete("clear")]
        public async Task<ActionResult> ClearCart()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return BadRequest("Invalid or missing user ID in token.");
            }

            await _cartService.ClearCart(userId);
            return NoContent();
        }
    }
}

