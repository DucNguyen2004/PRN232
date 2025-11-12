using System.ComponentModel.DataAnnotations;

public class DateGreaterThanAttribute : ValidationAttribute
{
    private readonly string _comparisonProperty;

    public DateGreaterThanAttribute(string comparisonProperty)
    {
        _comparisonProperty = comparisonProperty;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var currentValue = value as DateTime?;
        var comparisonProperty = validationContext.ObjectType.GetProperty(_comparisonProperty);

        if (comparisonProperty == null)
        {
            return new ValidationResult($"Không tìm thấy thuộc tính {_comparisonProperty} để so sánh.");
        }

        var comparisonValue = comparisonProperty.GetValue(validationContext.ObjectInstance) as DateTime?;

        if (currentValue.HasValue && comparisonValue.HasValue && currentValue <= comparisonValue)
        {
            return new ValidationResult(ErrorMessage ?? "Giá trị ngày không hợp lệ.");
        }

        return ValidationResult.Success;
    }
}
