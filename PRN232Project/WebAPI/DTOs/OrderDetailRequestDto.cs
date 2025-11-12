namespace DTOs
{
    public class OrderDetailRequestDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public List<int> ProductOptionIds { get; set; } = new List<int>();
        public int Price { get; set; }
    }
}
