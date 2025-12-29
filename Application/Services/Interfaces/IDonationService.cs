using Application.DTOs.AllRequests.Donations;

namespace Application.Services.Interfaces
{
    public interface IDonationService
    {
        Task RequestEquipmentAsync(RequestRejectDonationDto dto);
        Task RequestMedicineAsync(RequestRejectDonationDto dto);
        Task ApproveAssignEquipmentAsync(ApproveDonationDto dto);
        Task ApproveAssignMedicineAsync(ApproveDonationDto dto);
        Task RejectEquipmentAsync(RequestRejectDonationDto dto);
        Task RejectMedicineAsync(RequestRejectDonationDto dto);
        Task<int> GetAllDonationsAsync();
    }


}
