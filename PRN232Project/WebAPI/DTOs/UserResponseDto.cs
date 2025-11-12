namespace DTOs
{
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
}
