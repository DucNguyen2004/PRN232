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
}
