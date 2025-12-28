using Application.DTOs.Auth.Requests.RequestEquipment;
using Application.DTOs.Auth.Requests.RequestMedicine;
using Domain.Entities;

namespace Application.Services.Interfaces
{
    public interface IRequestService
    {
        Task CreateEquipmentRequestAsync(RequestEquipmentDto dto);
        Task CreateMedicineRequestAsync(RequestMedicineDto dto);

        Task ApproveEquipmentRequestAsync(int requestId);
        Task ApproveMedicineRequestAsync(int requestId);

        Task RejectEquipmentRequestAsync(int requestId);
        Task RejectMedicineRequestAsync(int requestId);

        Task<List<RequestEquipment>> GetPendingEquipmentRequests();
        Task<List<RequestMedicine>> GetPendingMedicineRequests();

        Task<List<RequestEquipment>> GetUnavailableEquipmentRequests();
        Task<List<RequestMedicine>> GetUnavailableMedicineRequests();

        Task AddEquipmentToCartAsync(int donationEquipmentId);
        Task AddMedicineToCartAsync(int donationMedicineId);
        Task RemoveEquipmentFromCartAsync(int donationEquipmentId);
        Task RemoveMedicineFromCartAsync(int donationMedicineId);

        Task<int> AllUnavailableDonationRequests();
    }
}
