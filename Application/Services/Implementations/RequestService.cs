using Application.DTOs.AllRequests.Requests;
using Application.DTOs.AllRequests.Requests.Carts;
using Application.Repositories.Interfaces;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Entities.Enum;
using Microsoft.EntityFrameworkCore;
using Application.Services.Interfaces.FileService;

namespace Application.Services.Implementations
{
    public class RequestService : IRequestService
    {
        private readonly IGenericRepository<RequestEquipment> _requestEquipmentRepo;
        private readonly IGenericRepository<RequestMedicine> _requestMedicineRepo;
        private readonly IGenericRepository<DonationEquipment> _donationEquipmentRepo;
        private readonly IGenericRepository<DonationMedicine> _donationMedicineRepo;
        private readonly IFileStorageService _fileStorageService;


        public RequestService(
            IGenericRepository<RequestEquipment> requestEquipmentRepo,
            IGenericRepository<RequestMedicine> requestMedicineRepo,
            IGenericRepository<DonationEquipment> donationEquipmentRepo,
            IGenericRepository<DonationMedicine> donationMedicineRepo,
            IFileStorageService fileStorageService
            )
        {
            _requestEquipmentRepo = requestEquipmentRepo;
            _requestMedicineRepo = requestMedicineRepo;
            _donationEquipmentRepo = donationEquipmentRepo;
            _donationMedicineRepo = donationMedicineRepo;
            _fileStorageService = fileStorageService;
        }

        public async Task CreateEquipmentRequestAsync(RequestUploadEquipmentDto dto)
        {
            var imagePaths = await _fileStorageService.UploadAsync(
                            dto.Images,
                            "Uploads/Requests/Equipments",
                            minFiles: 1,
                            maxFiles: 3
                        );
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
                Status = StatusDonation.Pending,
                Image1 = imagePaths[0],
                Image2 = imagePaths[1],
                Image3 = imagePaths[2],
             };

            await _requestEquipmentRepo.Insert(request);
            await _requestEquipmentRepo.SaveChanges();
        }

        public async Task CreateMedicineRequestAsync(RequestUploadMedicineDto dto)
        {
            var imagePaths = await _fileStorageService.UploadAsync(
                           dto.Images,
                           "Uploads/Requests/Medicines",
                           minFiles: 1,
                           maxFiles: 3
                       );
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
                Status = StatusDonation.Pending,
                Image1 = imagePaths[0],
                Image2 = imagePaths[1],
                Image3 = imagePaths[2],
            };

            await _requestMedicineRepo.Insert(request);
            await _requestMedicineRepo.SaveChanges();
        }

        public async Task ApproveEquipmentRequestAsync(RequestDto dto)
        {
            var request = await _requestEquipmentRepo.GetById(dto.RequestId);
            if (request == null) throw new Exception("Request not found");

            var donation = new DonationEquipment
            {
                ItemName = request.ItemName,
                ItemDesc = request.ItemDesc,
                Quantity = request.Quantity,
                Condition = request.Condition,
                Accessories = request.Accessories,
                IsAvailable = request.IsAvailable,
                UserId = request.UserId,
                CreationDate = DateTime.UtcNow,
                IsOrderd = false,
                Image1 = request.Image1,
                Image2 = request.Image2,
                Image3 = request.Image3
            };

            await _donationEquipmentRepo.Insert(donation);

            request.Status = StatusDonation.Approved;

            await _donationEquipmentRepo.SaveChanges();
            await _requestEquipmentRepo.SaveChanges();
        }

        public async Task ApproveMedicineRequestAsync(RequestDto dto)
        {
            var request = await _requestMedicineRepo.GetById(dto.RequestId);
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
                IsOrderd = false,
                Image1 = request.Image1,
                Image2 = request.Image2,
                Image3 = request.Image3,
            };

            await _donationMedicineRepo.Insert(donation);

            request.Status = StatusDonation.Approved;

            await _donationMedicineRepo.SaveChanges();
            await _requestMedicineRepo.SaveChanges();
        }


        public async Task RejectEquipmentRequestAsync(RequestDto dto)
        {
            var request = await _requestEquipmentRepo.GetById(dto.RequestId);
            if (request == null) throw new Exception("Request not found");

            request.Status = StatusDonation.Rejected;
            await _requestEquipmentRepo.SaveChanges();
        }

        public async Task RejectMedicineRequestAsync(RequestDto dto)
        {
            var request = await _requestMedicineRepo.GetById(dto.RequestId);
            if (request == null) throw new Exception("Request not found");

            request.Status = StatusDonation.Rejected;
            await _requestMedicineRepo.SaveChanges();
        }

        public async Task<List<DonationRequestStatusDto>> GetPendingEquipmentRequests()
        {
            return await _requestEquipmentRepo.GetAll()
                .Where(r => r.Status == StatusDonation.Pending)
                .Select(r => new DonationRequestStatusDto
                {
                    ItemName = r.ItemName,
                    Status = r.Status.Value
                })
                .ToListAsync();
        }


        public async Task<List<DonationRequestStatusDto>> GetPendingMedicineRequests()
        {
            return await _requestMedicineRepo.GetAll()
                .Where(r => r.Status == StatusDonation.Pending)
                .Select(r => new DonationRequestStatusDto
                {
                    ItemName = r.ItemName,
                    Status = r.Status.Value
                })
                .ToListAsync();
        }


        public async Task<List<DonationRequestStatusDto>> GetUnavailableEquipmentRequests()
        {
            return await _requestEquipmentRepo.GetAll()
                .Where(r => !r.IsAvailable)
                .Select(r => new DonationRequestStatusDto
                {
                    ItemName = r.ItemName,
                    Status = r.Status.Value
                })
                .ToListAsync();
        }


        public async Task<List<DonationRequestStatusDto>> GetUnavailableMedicineRequests()
        {
            return await _requestMedicineRepo.GetAll()
                .Where(r => !r.IsAvailable)
                .Select(r => new DonationRequestStatusDto
                {
                    ItemName = r.ItemName,
                    Status = r.Status.Value
                })
                .ToListAsync();
        }


        public async Task<int> AllUnavailableDonationRequests()
        {
            var eq = await GetUnavailableEquipmentRequests();
            var med = await GetUnavailableMedicineRequests();

            return eq.Count + med.Count;
        }


        public async Task AddEquipmentToCartAsync(AddToCartDto dto)
        {
            var donation = await _donationEquipmentRepo.GetById(dto.DonationId);
            if (donation == null) throw new Exception("Donation not found");
            donation.AddedToCart = true;
            _donationEquipmentRepo.Update(donation);
            await _donationEquipmentRepo.SaveChanges();
        }
        public async Task AddMedicineToCartAsync(AddToCartDto dto)
        {
            var donation = await _donationMedicineRepo.GetById(dto.DonationId);
            if (donation == null) throw new Exception("Donation not found");
            donation.AddedToCart = true;
            _donationMedicineRepo.Update(donation);
            await _donationMedicineRepo.SaveChanges();
        }
        public async Task RemoveEquipmentFromCartAsync(AddToCartDto dto)
        {
            var donation = await _donationEquipmentRepo.GetById(dto.DonationId);
            if (donation == null) throw new Exception("Donation not found");
            donation.AddedToCart = false;
            _donationEquipmentRepo.Update(donation);
            await _donationEquipmentRepo.SaveChanges();
        }
        public async Task RemoveMedicineFromCartAsync(AddToCartDto dto)
        {
            var donation = await _donationMedicineRepo.GetById(dto.DonationId);
            if (donation == null) throw new Exception("Donation not found");
            donation.AddedToCart = false;
            _donationMedicineRepo.Update(donation);
            await _donationMedicineRepo.SaveChanges();
        }
    }

}
