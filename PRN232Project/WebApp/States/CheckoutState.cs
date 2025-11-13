using DTOs;

namespace WebApp.States
{
    public class CheckoutState
    {
        public string? Message { get; set; }
        public List<CartItemResponseDto> CartData { get; set; } = new List<CartItemResponseDto>();
    }
}
