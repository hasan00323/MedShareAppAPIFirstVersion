using Application.DTOs.Auth.Requests.RequestEquipment;
using Application.DTOs.Auth.Requests.RequestMedicine;
using Application.Repositories.Interfaces;
using Domain.Entities;

namespace Application.Services.Interfaces
{
    public interface IRequestService
    {
        Task ApproveRequestAsync(int requestId);
        Task CreateEquipmentRequestAsync(RequestEquipmentDto dto);
        Task CreateMedicineRequestAsync(RequestMedicineDto dto);
        Task<IEnumerable<Request>> GetPendingRequestsForAdmin();
        Task<IEnumerable<Request>> GetUnAvailableRequests();
        Task<IEnumerable<Request>> GetRejectedRequests();
        Task RejectRequestAsync(int requestId);
    }

}
