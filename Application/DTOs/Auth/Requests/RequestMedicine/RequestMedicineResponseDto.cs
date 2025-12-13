using Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Auth.Requests.RequestMedicine
{
    public class RequestMedicineResponseDto
    {
        public int RequestId { get; set; }
        public int DonationId { get; set; }

        public string? Strength { get; set; }
        public DosageForm DosageForm { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string ItemName { get; set; }
        public string? PhotoURL { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
        public StatusDonation StatusDonation { get; set; }

    }
}

