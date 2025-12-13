using Domain.Entities.Enum;
using System.ComponentModel.DataAnnotations;
namespace Domain.Entities
{
    public class DonationMedicine: Donation
    {
        [Required]
        public string? Strength { get; set; }
        [Required]
        public DosageForm DosageForm { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }

     
        [Required]
        public bool UnOpend { get; set; } = true;
    }
   
}
