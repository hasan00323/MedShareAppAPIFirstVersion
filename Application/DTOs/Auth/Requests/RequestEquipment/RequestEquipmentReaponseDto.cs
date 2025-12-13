using Domain.Entities.Enum;

namespace Application.DTOs.Auth.Requests.RequestEquipment
{
    public class RequestEquipmentReaponseDto
    {
        public int RequestId { get; set; }
        public int DonationId { get; set; }
        public equStatus Condition { get; set; }
        public string? Accessories { get; set; }
        public string ItemName { get; set; }
        public string? PhotoURL { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
    }
}
