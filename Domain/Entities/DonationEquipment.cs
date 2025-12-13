using Domain.Entities.Enum;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class DonationEquipment: Donation
    {
        [Required]
        public equStatus Condition { get; set; }

        public string? Accessories { get; set; }
    }
}
