namespace DTOs
{
    public class CouponResponseDto
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

        public DateTime? DateValid { get; set; }

        public DateTime? DateExpired { get; set; }

        public int? MinOrderValue { get; set; }

        public string ValueType { get; set; }

        public int Value { get; set; }

        public ProductCouponResponseDto? Product { get; set; }

        public CategoryResponseDto? Category { get; set; }

        public List<int> UsageUsers { get; set; } = new List<int>();

        public int MaxUsages { get; set; } = 1;
    }

    public class ProductCouponResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
