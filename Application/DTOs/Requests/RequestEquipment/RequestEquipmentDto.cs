using Domain.Entities.Enum;

namespace Application.DTOs.Auth.Requests.RequestEquipment
{
    public class RequestEquipmentDto
    {
        public string ItemName { get; set; }
        public string? PhotoURL { get; set; }
        public int Quantity { get; set; }
        public equStatus Condition { get; set; }
        public string Accessories { get; set; }
        public bool IsAvailable { get; set; }
        public int DonationId { get; set; }
        public int UserId { get; set; }

    }
}
