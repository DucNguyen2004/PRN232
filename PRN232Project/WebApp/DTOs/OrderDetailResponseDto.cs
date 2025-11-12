namespace DTOs
{
    public class OrderDetailResponseDto
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public ProductOrderDetailResponseDto Product { get; set; }
        public List<ProductOptionResponseDto> ProductOptions { get; set; } = new List<ProductOptionResponseDto>();
    }

    public class ProductOrderDetailResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }
}