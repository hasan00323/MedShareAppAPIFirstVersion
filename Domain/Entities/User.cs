using Domain.Entities.Enum;
using System.ComponentModel.DataAnnotations;
namespace Domain.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required,StringLength(50,MinimumLength =3,ErrorMessage ="Invalid Name.")]
        public string? FullName { get; set; }

        [Required, EmailAddress, StringLength(70, MinimumLength = 10, ErrorMessage = "Invalid Email.")]
        public string? Email { get; set; }

        [MaxLength(500)]
        public string? Password { get; set; }

        public string? PhoneNumber { get; set; }
        public Role Role { get; set; }
        public ICollection<Donation>? MyDonations { get; set; }
        public ICollection<Request>? MyRequests { get; set; }
    }
   
}
