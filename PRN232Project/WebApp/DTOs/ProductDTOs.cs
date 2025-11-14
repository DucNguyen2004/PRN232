using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class ProductRequestDto
    {
        [Required(ErrorMessage = "Tên sản phẩm không được để trống.")]
        [MaxLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Danh mục không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "Danh mục không hợp lệ.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Mô tả sản phẩm không được để trống.")]
        [MaxLength(500, ErrorMessage = "Mô tả không được dài quá 500 ký tự.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm không được để trống.")]
        [Range(0, int.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn hoặc bằng 0.")]
        public int Price { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Trạng thái sản phẩm không được để trống.")]
        [RegularExpression("ACTIVE|INACTIVE", ErrorMessage = "Trạng thái chỉ có thể là ACTIVE hoặc INACTIVE.")]
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
