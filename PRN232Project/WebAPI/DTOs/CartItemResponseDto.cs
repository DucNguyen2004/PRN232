namespace DTOs
{
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
}
