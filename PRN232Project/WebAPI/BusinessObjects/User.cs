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

        public ICollection<Role> Roles { get; set; }

        public ICollection<Order> Orders { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

    }

}
