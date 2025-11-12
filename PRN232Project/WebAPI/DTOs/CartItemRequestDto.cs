namespace DTOs
{
    public class CartItemRequestDto
    {
        public int ProductId { get; set; }
        public List<int> ProductOptionIds { get; set; } = new List<int>();
        public int Quantity { get; set; }
    }
}
