
using Domain.Entities.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Donation
    {
        [Key]
        public int DonationId { get; set; }
        [Required(ErrorMessage = "This field is required"), StringLength(50, MinimumLength = 3, ErrorMessage = "Invalid Name.")]
        public string? ItemName { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.ImageUrl)]
        public string? PhotoURL { get; set; }
        public bool IsOrderd { get; set; } = false;
        public bool IsAvailable { get; set; } = true;
        public AssignStatus AssignStatus { get; set; }
        [Required]
        public int Quantity { get; set; }

        public int UserId { get; set; }
        public User? Donor { get; set; }

        public int? Receiver { get; set; }
        public int? UserAssignedTo { get; set; }
    }
}
