using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class ProductRequestDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string Description { get; set; }
        public int Price { get; set; }
        public DateTime CreateAt { get; set; }
        [Required]
        public string Status { get; set; }
        public ICollection<string> Images { get; set; } = new List<string>();
        public ICollection<ProductOptionRequestDto> Options { get; set; } = new List<ProductOptionRequestDto>();
    }

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
        public string Name { get; set; }
        public CategoryResponseDto? Category { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public DateTime CreateAt { get; set; }
        public int Sold { get; set; }
        public string Status { get; set; }
        public ICollection<string> Images { get; set; } = new List<string>();
        public ICollection<ProductOptionResponseDto> Options { get; set; } = new List<ProductOptionResponseDto>();
    }

    public class ProductOptionRequestDto
    {
        public int OptionValueId { get; set; }
        public int DeltaPrice { get; set; }
    }

    public class ProductOptionResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int DeltaPrice { get; set; }
    }
}
