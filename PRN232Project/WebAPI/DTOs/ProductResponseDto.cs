namespace DTOs
{
    public class ProductResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public DateTime CreateAt { get; set; }
        public string Status { get; set; }
        public string PrevStatus { get; set; }
    }

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
    }
}
