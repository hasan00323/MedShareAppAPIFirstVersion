using Application.DTOs.Auth.Requests.RequestEquipment;
using Application.DTOs.Auth.Requests.RequestMedicine;
using Application.Repositories.Interfaces;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Entities.Enum;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    public class RequestService : IRequestService
    {
        private readonly IGenericRepository<RequestEquipment> _requestEquipmentRepo;
        private readonly IGenericRepository<RequestMedicine> _requestMedicineRepo;
        private readonly IGenericRepository<DonationEquipment> _donationEquipmentRepo;
        private readonly IGenericRepository<DonationMedicine> _donationMedicineRepo;

        public RequestService(
            IGenericRepository<RequestEquipment> requestEquipmentRepo,
            IGenericRepository<RequestMedicine> requestMedicineRepo,
            IGenericRepository<DonationEquipment> donationEquipmentRepo,
            IGenericRepository<DonationMedicine> donationMedicineRepo)
        {
            _requestEquipmentRepo = requestEquipmentRepo;
            _requestMedicineRepo = requestMedicineRepo;
            _donationEquipmentRepo = donationEquipmentRepo;
            _donationMedicineRepo = donationMedicineRepo;
        }

        public async Task CreateEquipmentRequestAsync(RequestEquipmentDto dto)
        {
            var request = new RequestEquipment
            {
                ItemName = dto.ItemName,
                ItemDesc = dto.ItemDesc,
                Quantity = dto.Quantity,
                Condition = dto.Condition,
                Accessories = dto.Accessories!,
                IsAvailable = dto.IsAvailable,
                UserId = dto.UserId,
                CreationDate = DateTime.UtcNow,
                Status = StatusDonation.Pending
            };

            await _requestEquipmentRepo.Insert(request);
            await _requestEquipmentRepo.SaveChanges();
        }

        public async Task CreateMedicineRequestAsync(RequestMedicineDto dto)
        {
            var request = new RequestMedicine
            {
                ItemName = dto.ItemName,
                ItemDesc = dto.ItemDesc,
                Quantity = dto.Quantity,
                Strength = dto.Strength,
                DosageForm = dto.DosageForm,
                ExpirationDate = dto.ExpirationDate,
                UnOpend = dto.UnOpend,
                IsAvailable = dto.IsAvailable,
                UserId = dto.UserId,
                CreationDate = DateTime.UtcNow,
                Status = StatusDonation.Pending
            };

            await _requestMedicineRepo.Insert(request);
            await _requestMedicineRepo.SaveChanges();
        }

        public async Task ApproveEquipmentRequestAsync(int requestId)
        {
            var request = await _requestEquipmentRepo.GetById(requestId);
            if (request == null) throw new Exception("Request not found");

            var donation = new DonationEquipment
            {
                ItemName = request.ItemName,
                ItemDesc = request.ItemDesc,
                Quantity = request.Quantity,
                Condition = request.Condition ,
                Accessories = request.Accessories,
                IsAvailable = request.IsAvailable,
                UserId = request.UserId,
                CreationDate = DateTime.UtcNow,
                IsOrderd = false
            };

            await _donationEquipmentRepo.Insert(donation);

            request.Status = StatusDonation.Approved;

            await _donationEquipmentRepo.SaveChanges();
            await _requestEquipmentRepo.SaveChanges();
        }

        public async Task ApproveMedicineRequestAsync(int requestId)
        {
            var request = await _requestMedicineRepo.GetById(requestId);
            if (request == null) throw new Exception("Request not found");

            var donation = new DonationMedicine
            {
                ItemName = request.ItemName,
                ItemDesc = request.ItemDesc,
                Quantity = request.Quantity,
                Strength = request.Strength,
                DosageForm = request.DosageForm!,
                ExpirationDate = request.ExpirationDate!,
                UnOpend = request.UnOpend ?? true,
                IsAvailable = request.IsAvailable,
                UserId = request.UserId,
                CreationDate = DateTime.UtcNow,
                IsOrderd = false
            };

            await _donationMedicineRepo.Insert(donation);

            request.Status = StatusDonation.Approved;

            await _donationMedicineRepo.SaveChanges();
            await _requestMedicineRepo.SaveChanges();
        }


        public async Task RejectEquipmentRequestAsync(int requestId)
        {
            var request = await _requestEquipmentRepo.GetById(requestId);
            if (request == null) throw new Exception("Request not found");

            request.Status = StatusDonation.Rejected;
            await _requestEquipmentRepo.SaveChanges();
        }

        public async Task RejectMedicineRequestAsync(int requestId)
        {
            var request = await _requestMedicineRepo.GetById(requestId);
            if (request == null) throw new Exception("Request not found");

            request.Status = StatusDonation.Rejected;
            await _requestMedicineRepo.SaveChanges();
        }

        public async Task<List<RequestEquipment>> GetPendingEquipmentRequests()
            => await _requestEquipmentRepo.GetAll()
                .Where(r => r.Status == StatusDonation.Pending)
                .ToListAsync();

        public async Task<List<RequestMedicine>> GetPendingMedicineRequests()
            => await _requestMedicineRepo.GetAll()
                .Where(r => r.Status == StatusDonation.Pending)
                .ToListAsync();

        public async Task<List<RequestEquipment>> GetUnavailableEquipmentRequests()
            => await _requestEquipmentRepo.GetAll()
                .Where(r => !r.IsAvailable)
                .ToListAsync();

        public async Task<List<RequestMedicine>> GetUnavailableMedicineRequests()
            => await _requestMedicineRepo.GetAll()
                .Where(r => !r.IsAvailable)
                .ToListAsync();


        public async Task<int> AllUnavailableDonationRequests()
        {
            var eq = await GetUnavailableEquipmentRequests();
            var med = await GetUnavailableMedicineRequests();

            return eq.Count + med.Count;
        }


        public async Task AddEquipmentToCartAsync(int donationEquipmentId)
        {
            var donation = await _donationEquipmentRepo.GetById(donationEquipmentId);
            if (donation == null) throw new Exception("Donation not found");
            donation.AddedToCart = true;
            _donationEquipmentRepo.Update(donation);
            await _donationEquipmentRepo.SaveChanges();
        }
        public async Task AddMedicineToCartAsync(int donationMedicineId)
        {
            var donation = await _donationMedicineRepo.GetById(donationMedicineId);
            if (donation == null) throw new Exception("Donation not found");
            donation.AddedToCart = true;
            _donationMedicineRepo.Update(donation);
            await _donationMedicineRepo.SaveChanges();
        }
        public async Task RemoveEquipmentFromCartAsync(int donationEquipmentId)
        {
            var donation = await _donationEquipmentRepo.GetById(donationEquipmentId);
            if (donation == null) throw new Exception("Donation not found");
            donation.AddedToCart = false;
            _donationEquipmentRepo.Update(donation);
            await _donationEquipmentRepo.SaveChanges();
        }
        public async Task RemoveMedicineFromCartAsync(int donationMedicineId)
        {
            var donation = await _donationMedicineRepo.GetById(donationMedicineId);
            if (donation == null) throw new Exception("Donation not found");
            donation.AddedToCart = false;
            _donationMedicineRepo.Update(donation);
            await _donationMedicineRepo.SaveChanges();
        }
    }

}
