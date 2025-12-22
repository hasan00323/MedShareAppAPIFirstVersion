
namespace Application.DTOs.Auth.Requests.GetDonation
{
    public class GetDonationRequestDto
    {
        public int UserId { get; set; }
        public int DonationId { get; set; }
        public int Quantity { get; set; }
    }
}
