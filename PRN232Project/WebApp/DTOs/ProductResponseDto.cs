namespace DTOs
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public CategoryResponseDto? Category { get; set; }
        public required string Description { get; set; }
        public int Price { get; set; }
        public DateTime CreateAt { get; set; }
        public int Sold { get; set; }
        public required string Status { get; set; }
        public ICollection<string> Images { get; set; } = new List<string>();
        public ICollection<ProductOptionResponseDto> Options { get; set; } = new List<ProductOptionResponseDto>();
        public string MainImage { get; set; }
    }
}
