using Domain.Entities.Enum;
using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Auth.Requests.RequestEquipment
{
    public class RequestEquipmentDto
    {
        public string ItemName { get; set; }
        public string? ItemDesc { get; set; }
        public List<IFormFile> Images { get; set; } = new();
        public int Quantity { get; set; }
        public equStatus Condition { get; set; }
        public string? Accessories { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsWokringFully { get; set; }
        public int UserId { get; set; }

    }
}
