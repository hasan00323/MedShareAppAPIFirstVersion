using Application.Repositories.Interfaces;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Entities.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Services.Implementations
{
    public class DonationService : IDonationService
    {
        private readonly IGenericRepository<Donation> _donationRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DonationService(IGenericRepository<Donation> donationRepo,IHttpContextAccessor httpContextAccessor)
        {
            _donationRepo = donationRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task RequestDonationAsync(int donationId, int userId)
        {
            var donation = await _donationRepo.GetById(donationId);

            if (donation == null)
                throw new Exception("Donation not found");

            if (!donation.IsAvailable || donation.IsOrderd)
                throw new Exception("Donation already requested");

            donation.IsOrderd = true;
            donation.AssignStatus = AssignStatus.Pending;
            donation.Receiver = userId;

            await _donationRepo.SaveChanges();
        }

        public async Task ApproveDonationRequestAsync(int donationId,int Quantity,int userId)//edited
        {
            var donation = await _donationRepo.GetById(donationId);

            if (donation == null)
                throw new Exception("Donation not found");

            if (donation.IsOrderd)
                throw new Exception("Donation was requested");

            if (donation.Quantity < Quantity)
            
                throw new Exception("Requested quantity exceeds available donation quantity.");
        

            donation.Quantity= donation.Quantity - Quantity;
            donation.AssignStatus = AssignStatus.AssigningApproved;

            if (donation.Quantity == 0)

                donation.IsAvailable = false;

            else

                donation.IsAvailable = true;

            if (donation.UserAssignedTo !=0 || donation.UserAssignedTo != null)

                throw new Exception("This Donation is Assigned to another user.");

            donation.UserAssignedTo = userId;

            await _donationRepo.SaveChanges();
        }

        public async Task RejectDonationRequestAsync(int donationId)
        {
            var donation = await _donationRepo.GetById(donationId);

            if (donation == null)
                throw new Exception("Donation not found");

            donation.IsOrderd = false;
            donation.Receiver = null;
            donation.AssignStatus = AssignStatus.AssigningRejected;

            await _donationRepo.SaveChanges();
        }

        public async Task AddDonationToCart(int donationId)
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User not authenticated");

            var userId = Convert.ToInt32(userIdClaim);

            var donation = await _donationRepo.GetAll()
                .FirstOrDefaultAsync(d => d.DonationId == donationId);

            if (donation == null)
                throw new Exception("Donation not found");

            if (!donation.IsAvailable || donation.IsOrderd)
                throw new InvalidOperationException("Donation already ordered");

            donation.IsOrderd = true;
            donation.Receiver = userId;

            _donationRepo.Update(donation);
            await _donationRepo.SaveChanges();
        }

    }

}
