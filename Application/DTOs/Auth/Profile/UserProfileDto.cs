
namespace Application.DTOs.Auth.Profile
{
    public class UserProfileDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ImageProfile { get; set; }
    }
}
