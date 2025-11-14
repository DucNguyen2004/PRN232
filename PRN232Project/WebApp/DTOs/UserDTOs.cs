using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class UserRequestDto
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập.")]
        [MinLength(4, ErrorMessage = "Tên đăng nhập phải có ít nhất 4 ký tự.")]
        public string Username { get; set; }

        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên.")]
        public string Fullname { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        [RegularExpression(@"^\d{10}$",
            ErrorMessage = "Số điện thoại phải có từ 9 đến 15 chữ số.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Dob { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn giới tính.")]
        public string Gender { get; set; }

        public string Status { get; set; } = "ACTIVE";

        public List<int> RoleIds { get; set; } = new List<int>();

        public DateTime CreatedAt { get; set; }

    }

    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? Dob { get; set; }
        public string Gender { get; set; }
        public string Status { get; set; } = "ACTIVE";
        public List<int> RoleIds { get; set; } = new List<int>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class IsUserExistedDto
    {
        public bool UserNameExists { get; set; }
        public bool EmailExists { get; set; }
        public bool PhoneExists { get; set; }
    }
}
