using Application.Repositories.Interfaces;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Entities.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class DonationService : IDonationService
    {
        private readonly IGenericRepository<Donation> _donationRepo;

        public DonationService(IGenericRepository<Donation> donationRepo)
        {
            _donationRepo = donationRepo;
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

        public async Task ApproveDonationRequestAsync(int donationId)
        {
            var donation = await _donationRepo.GetById(donationId);

            if (donation == null)
                throw new Exception("Donation not found");

            if (!donation.IsOrderd)
                throw new Exception("Donation was not requested");

            donation.AssignStatus = AssignStatus.AssigningApproved;
            donation.IsAvailable = false;

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
    }

}
