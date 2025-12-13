using Application.DTOs.Auth.Requests.GetDonation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IDonationService
    {
        Task RequestDonationAsync(int donationId, int userId);
        Task ApproveDonationRequestAsync(int donationId, int Quantity);
        Task RejectDonationRequestAsync(int donationId);
    }

}
