using Domain.Entities.Enum;
using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Auth.Requests.RequestMedicine
{
    public class RequestMedicineDto
    {
        public string ItemName { get; set; }
        public string? ItemDesc { get; set; }
        public List<IFormFile> Images { get; set; } = new();
        public string? Strength { get; set; }
        public int Quantity { get; set; }
        public DosageForm DosageForm { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsAvailable { get; set; }
        public bool UnOpend { get; set; }
        public int UserId { get; set; }
    }
}
