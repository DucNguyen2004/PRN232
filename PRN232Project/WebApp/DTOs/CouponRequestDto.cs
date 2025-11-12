using System.ComponentModel.DataAnnotations;

public class CouponRequestDto
{
    [Required(ErrorMessage = "Mã giảm giá là bắt buộc.")]
    [StringLength(50, ErrorMessage = "Mã giảm giá không được vượt quá 50 ký tự.")]
    public string Code { get; set; }

    [Required(ErrorMessage = "Tên là bắt buộc.")]
    [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự.")]
    public string Name { get; set; }

    [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc.")]
    [RegularExpression("ACTIVE|INACTIVE", ErrorMessage = "Trạng thái phải là ACTIVE hoặc INACTIVE.")]
    public string Status { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    [Required(ErrorMessage = "Ngày hiệu lực là bắt buộc.")]
    [DataType(DataType.Date)]
    public DateTime? DateValid { get; set; }

    [Required(ErrorMessage = "Ngày hết hạn là bắt buộc.")]
    [DataType(DataType.Date)]
    [DateGreaterThan("DateValid", ErrorMessage = "Ngày hết hạn phải sau ngày bắt đầu.")]
    public DateTime? DateExpired { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Giá trị đơn hàng tối thiểu phải lớn hơn hoặc bằng 0.")]
    public int? MinOrderValue { get; set; }

    [Required(ErrorMessage = "Loại giá trị là bắt buộc.")]
    [RegularExpression("PERCENT|FIXED", ErrorMessage = "Loại giá trị phải là PERCENT hoặc FIXED.")]
    public string ValueType { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Giá trị phải lớn hơn 0.")]
    public int Value { get; set; }

    public int? ProductId { get; set; }

    public int? CategoryId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Số lượt sử dụng tối đa phải lớn hơn 0.")]
    public int MaxUsages { get; set; } = 1;
}
