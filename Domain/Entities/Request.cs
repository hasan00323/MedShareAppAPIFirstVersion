using Domain.Entities.Enum;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Request
    {
        [Key]
        public int RequestId { get; set; }

        public string? ItemName { get; set; }
     
        public string? ItemDesc { get; set; }

        public bool IsAvailable { get; set; }

        public string? PhotoURL { get; set; }

        public int? Quantity { get; set; }

        public equStatus? Condition { get; set; }

        public string? Accessories { get; set; }

        public string? Strength { get; set; }

        public DosageForm? DosageForm { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public bool? UnOpend { get; set; }

        public bool IsEquipment { get; set; }

        public StatusDonation? Status { get; set; } = StatusDonation.Pending;
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int? DonationId { get; set; }
        public Donation? Donation { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
