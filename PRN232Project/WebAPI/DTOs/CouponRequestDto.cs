namespace DTOs
{
    public class CouponRequestDto
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }
        public DateTime DateCreated { get; set; }

        public DateTime? DateValid { get; set; }

        public DateTime? DateExpired { get; set; }

        public int? MinOrderValue { get; set; }

        public string ValueType { get; set; }

        public int Value { get; set; }

        public int? ProductId { get; set; }

        public int? CategoryId { get; set; }

        public int MaxUsages { get; set; } = 1;
    }
}
