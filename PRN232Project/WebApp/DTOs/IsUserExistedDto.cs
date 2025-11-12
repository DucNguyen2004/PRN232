namespace DTOs
{
    public class IsUserExistedDto
    {
        public bool UserNameExists { get; set; }
        public bool EmailExists { get; set; }
        public bool PhoneExists { get; set; }
    }
}
