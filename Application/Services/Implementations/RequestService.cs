using Application.DTOs.Auth.Requests.RequestEquipment;
using Application.DTOs.Auth.Requests.RequestMedicine;
using Application.Repositories.Interfaces;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Entities.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class RequestService: IRequestService
    {
        private readonly IGenericRepository<Donation> _donationRepository;
        private readonly IGenericRepository<Request> _RequestRepository;
        public RequestService(IGenericRepository<Donation> donationRepository, IGenericRepository<Request> requestRepository)
        {
            _donationRepository = donationRepository;
            _RequestRepository = requestRepository;
        }

        public async Task ApproveRequestAsync(int requestId)
        {
          var request = await _RequestRepository.GetAll()
         .Include(r => r.User).FirstOrDefaultAsync(r => r.RequestId == requestId);
           
            if (request == null)  throw new Exception("Request not found");

            if (!request.IsEquipment)
            {
                var donationMedicine = new DonationMedicine
                {
                    ItemName = request.ItemName,
                    PhotoURL = request.PhotoURL,
                    Quantity = (int)request.Quantity!,
                    Strength = request.Strength,
                    DosageForm = (DosageForm)request.DosageForm!,
                    ExpirationDate = (DateTime)request.ExpirationDate!,
                    UnOpend = (bool)request.UnOpend!,
                    UserId = request.UserId,
                };
            }
            else
            {
                var donationEquipment = new DonationEquipment
                {
                    ItemName = request.ItemName,
                    PhotoURL = request.PhotoURL,
                    Quantity = (int)request.Quantity!,
                    Condition = (equStatus)request.Condition!,
                    Accessories = request.Accessories,
                    UserId = request.UserId,
                    IsAvailable = request.IsAvailable,
                    DonationId = (int)request.DonationId!,
                };
            }
            request.Status = StatusDonation.Approved;
            await _donationRepository.SaveChanges();
        }

        public async Task CreateEquipmentRequestAsync(RequestEquipmentDto dto)
        {
            var request = new Request
            {
                ItemName = dto.ItemName,
                UserId = dto.UserId,
                IsAvailable = dto.IsAvailable,
                DonationId = dto.DonationId,
                Status = StatusDonation.Pending,
                Condition = dto.Condition,
                Accessories = dto.Accessories,
                Quantity= dto.Quantity,
                PhotoURL= dto.PhotoURL,
                IsEquipment= true,
            };

            await _RequestRepository.Insert(request);
            await _RequestRepository.SaveChanges();
        }

        public async Task CreateMedicineRequestAsync(RequestMedicineDto dto)
        {
            var request = new Request
            {
                ItemName = dto.ItemName,
                UserId = dto.UserId,
                IsAvailable = dto.IsAvailable,
                DonationId = dto.DonationId,
                Status = StatusDonation.Pending,
                PhotoURL = dto.PhotoURL,
                Strength = dto.Strength,
                DosageForm = dto.DosageForm,
                ExpirationDate = dto.ExpirationDate,
                UnOpend = dto.UnOpend,
                IsEquipment= false,
            };

            await _RequestRepository.Insert(request);
            await _RequestRepository.SaveChanges();
        }

        public Task<IEnumerable<Request>> GetPendingRequestsForAdmin()
        {
            return Task.FromResult(_RequestRepository
                .GetAll()
                .Where(r => r.Status == StatusDonation.Pending)
                .AsEnumerable());
        }

        public Task<IEnumerable<Request>> GetUnAvailableRequests()
        {
            return Task.FromResult(_RequestRepository
                .GetAll()
                .Where(r => r.IsAvailable==false)
                .AsEnumerable());
        }

        public Task<IEnumerable<Request>> GetRejectedRequests()
        {
            return Task.FromResult(_RequestRepository
                .GetAll()
                .Where(r => r.Status == StatusDonation.Rejected)
                .AsEnumerable());
        }

        public async Task RejectRequestAsync(int requestId)
        {
            var request = await _RequestRepository.GetById(requestId);
            if (request == null)
                throw new Exception("Request not found");

            request.Status = StatusDonation.Rejected;
            await _RequestRepository.SaveChanges();
        }

    }
}
