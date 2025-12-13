using Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Auth.Requests.RequestEquipment
{
    public class RequestEquipmentDto
    {
        public string ItemName { get; set; }
        public string? PhotoURL { get; set; }
        public int Quantity { get; set; }
        public equStatus Condition { get; set; }
        public string? Accessories { get; set; }
        public bool IsAvailable { get; set; }
        public int? DonationId { get; set; }
        public int UserId { get; set; }

    }
}
