namespace DTOs
{
    public class LoginRequestDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class LoginResponseDto
    {
        public int UserId { get; set; }
        public string? Email { get; set; }
        public string? AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string? RefreshToken { get; set; }
    }
}
