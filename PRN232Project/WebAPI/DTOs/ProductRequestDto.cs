using BusinessObjects;

namespace DTOs
{
    public class ProductRequestDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> ImageList { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public Category Category { get; set; } // re-check
        public DateTime CreateAt { get; set; }
        public List<long> OptionList { get; set; }
        public string Status { get; set; }
        public string PrevStatus { get; set; }
        private User User { get; set; }
    }

    public class ProductRequestDto
    {
        public required string Name { get; set; }
        public required int CategoryId { get; set; }
        public required string Description { get; set; }
        public int Price { get; set; }
        public DateTime CreateAt { get; set; }
        public required string Status { get; set; }
        public ICollection<string> Images { get; set; } = new List<string>();
        public ICollection<ProductOptionRequestDto> Options { get; set; } = new List<ProductOptionRequestDto>();
    }
}
