using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects
{
    [Table("coupon")]
    public class Coupon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Status { get; set; } // e.g., ACTIVE, INACTIVE

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public DateTime? DateUpdated { get; set; }

        public DateTime? DateValid { get; set; }

        public DateTime? DateExpired { get; set; }

        // Minimum order total to apply this coupon
        public int? MinOrderValue { get; set; }

        // Discount type: percent or fixed
        public string ValueType { get; set; } // e.g., "percent", "fixed"

        public int Value { get; set; }

        // Optional: Apply to a specific product or category
        public int? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public int MaxUsages { get; set; } = 1;

        // Many users can use this coupon (1 time per user)
        public ICollection<CouponUsage> Usages { get; set; }
    }
}
