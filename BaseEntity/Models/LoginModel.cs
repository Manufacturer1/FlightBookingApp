namespace BaseEntity.Models
{
    public class LoginModel
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string Role { get; set; } = "user";
    }
}
