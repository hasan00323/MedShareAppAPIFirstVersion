using Application.DTOs.AllRequests.Requests;
using Application.DTOs.AllRequests.Requests.Carts;

namespace Application.Services.Interfaces
{
    public interface IRequestService
    {
        Task CreateEquipmentRequestAsync(RequestUploadEquipmentDto dto);
        Task CreateMedicineRequestAsync(RequestUploadMedicineDto dto);

        Task ApproveEquipmentRequestAsync(RequestDto dto);
        Task ApproveMedicineRequestAsync(RequestDto dto);

        Task RejectEquipmentRequestAsync(RequestDto dto);
        Task RejectMedicineRequestAsync(RequestDto dto);

        Task<List<DonationRequestStatusDto>> GetPendingEquipmentRequests();
        Task<List<DonationRequestStatusDto>> GetPendingMedicineRequests();

        Task<List<DonationRequestStatusDto>> GetUnavailableEquipmentRequests();
        Task<List<DonationRequestStatusDto>> GetUnavailableMedicineRequests();

        Task AddEquipmentToCartAsync(AddToCartDto dto);
        Task AddMedicineToCartAsync(AddToCartDto dto);
        Task RemoveEquipmentFromCartAsync(AddToCartDto dto);
        Task RemoveMedicineFromCartAsync(AddToCartDto dto);

        Task<int> AllUnavailableDonationRequests();
    }
}
