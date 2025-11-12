using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects
{
    [Table("products")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [Required]
        [MaxLength(256)]
        public string Description { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public DateTime CreateAt { get; set; } = DateTime.Now;

        public int Sold { get; set; } = 0;

        public string Status { get; set; }

        public string PrevStatus { get; set; }

        // One-to-Many relationships
        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<WishlistItem> WishlistItems { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<ProductOption> ProductOptions { get; set; }
    }
}