
namespace Application.Services.Interfaces
{
    public interface IDonationService
    {
        Task RequestDonationAsync(int donationId, int userId);
        Task ApproveDonationRequestAsync(int donationId, int Quantity, int userId);
        Task RejectDonationRequestAsync(int donationId);
        Task AddDonationToCart(int donationId);
    }

}
