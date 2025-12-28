namespace Application.Services.Interfaces
{
    public interface IDonationService
    {
        Task RequestEquipmentAsync(int donationId);
        Task RequestMedicineAsync(int donationId);
        Task ApproveAssignEquipmentAsync(int donationId, int quantity, int receiverUserId);
        Task ApproveAssignMedicineAsync(int donationId, int quantity, int receiverUserId);
        Task RejectEquipmentAsync(int donationId);
        Task RejectMedicineAsync(int donationId);
        Task<int> GetAllDonationsAsync();
    }


}
