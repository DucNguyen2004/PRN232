namespace DTOs
{
    public class UserCartResponseDto
    {
        public int UserId { get; set; }
        public List<CartItemResponseDto> CartItems { get; set; } = new List<CartItemResponseDto>();
    }
}
