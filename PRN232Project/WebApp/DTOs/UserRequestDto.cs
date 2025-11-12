using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class UserRequestDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        public string Password { get; set; }

        [Required]
        public string Fullname { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Phone must be 10 digits and start with 0.")]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        public DateTime? Dob { get; set; }

        [Required]
        public string Gender { get; set; } = "Male";
        public string Status { get; set; } = "ACTIVE";
        public List<int> RoleIds { get; set; } = new List<int>();
        public DateTime CreatedAt { get; set; }
    }
}
