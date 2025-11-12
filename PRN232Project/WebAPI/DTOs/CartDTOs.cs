namespace DTOs
{
    public class CartItemRequestDto
    {
        public int ProductId { get; set; }
        public List<int> ProductOptionIds { get; set; } = new List<int>();
        public int Quantity { get; set; }
    }

    public class CartItemResponseDto
    {
        public int Id { get; set; }
        public CartItemProductDto? Product { get; set; }
        public List<ProductOptionResponseDto>? ProductOptions { get; set; }
        public int Quantity { get; set; }
    }

    public class CartItemProductDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Price { get; set; }
        public required string Status { get; set; }
        public required string Image { get; set; }
        public required int CategoryId { get; set; }
    }

    public class UserCartResponseDto
    {
        public int UserId { get; set; }
        public List<CartItemResponseDto> CartItems { get; set; } = new List<CartItemResponseDto>();
    }
}
