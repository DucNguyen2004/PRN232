using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects
{
    public class CouponUsage
    {
        [Key]
        public int Id { get; set; }

        public int CouponId { get; set; }

        [ForeignKey("CouponId")]
        public Coupon Coupon { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public DateTime UsedAt { get; set; } = DateTime.UtcNow;
    }

}
