namespace BusinessObjects
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [MaxLength(256)]
        public string Password { get; set; }

        [Required]
        public bool IsHavePassword { get; set; } = false;

        [Required]
        [MaxLength(50)]
        public string Fullname { get; set; }

        [MaxLength(10)]
        public string Phone { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        public DateTime? Dob { get; set; }

        [MaxLength(10)]
        public string Gender { get; set; }

        [MaxLength(20)]
        public string Status { get; set; } = "ACTIVE";

        [MaxLength(1)]
        public bool IsActivated { get; set; } = false;

        // Many-to-Many: UserRoles
        public ICollection<Role> Roles { get; set; }

        // One-to-Many: Addresses
        public ICollection<UserAddress> Addresses { get; set; }

        // One-to-Many: Orders
        public ICollection<Order> Orders { get; set; }

        // One-to-Many: Refresh Tokens
        //public ICollection<ActiveRefreshToken> ActiveRefreshTokens { get; set; }

        // One-to-Many: Verification Codes
        //public ICollection<VerificationCode> VerificationCodes { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        // Logic to set IsHavePassword in EF Core can be added in service or DTO mapping
        public ICollection<CouponUsage> CouponUsages { get; set; } = new List<CouponUsage>();
    }

}
