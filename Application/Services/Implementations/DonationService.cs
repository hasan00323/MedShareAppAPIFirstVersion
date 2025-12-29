using Application.DTOs.AllRequests.Donations;
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
        private readonly IGenericRepository<DonationEquipment> _equipmentRepo;
        private readonly IGenericRepository<DonationMedicine> _medicineRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DonationService(
            IGenericRepository<DonationEquipment> equipmentRepo,
            IGenericRepository<DonationMedicine> medicineRepo,
            IHttpContextAccessor httpContextAccessor)
        {
            _equipmentRepo = equipmentRepo;
            _medicineRepo = medicineRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetCurrentUserId()
        {
            var claim = _httpContextAccessor.HttpContext?
                .User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (claim == null)
                throw new UnauthorizedAccessException("User not authenticated");

            return Convert.ToInt32(claim);
        }

        public async Task RequestEquipmentAsync(RequestRejectDonationDto dto)
        {
            var userId = GetCurrentUserId();
            var donation = await _equipmentRepo.GetById(dto.DonationId);

            if (donation == null)
                throw new Exception("Donation not found");

            if (!donation.IsAvailable || donation.IsOrderd)
                throw new Exception("Donation already requested");

            donation.IsOrderd = true;
            donation.AssignStatus = AssignStatus.Pending;
            donation.UserAssignedTo = userId;

            _equipmentRepo.Update(donation);
            await _equipmentRepo.SaveChanges();
        }

        public async Task RequestMedicineAsync(RequestRejectDonationDto dto)
        {
            var userId = GetCurrentUserId();
            var donation = await _medicineRepo.GetById(dto.DonationId);

            if (donation == null)
                throw new Exception("Donation not found");

            if (!donation.IsAvailable || donation.IsOrderd)
                throw new Exception("Donation already requested");

            donation.IsOrderd = true;
            donation.AssignStatus = AssignStatus.Pending;
            donation.UserAssignedTo = userId;

            _medicineRepo.Update(donation);
            await _medicineRepo.SaveChanges();
        }

        public async Task ApproveAssignEquipmentAsync(ApproveDonationDto dto)
        {
            var donation = await _equipmentRepo.GetById(dto.DonationId);

            if (donation == null)
                throw new Exception("Donation not found");

            if (donation.Quantity < dto.Quantity)
                throw new Exception("Quantity exceeds available amount");

            if (donation.UserAssignedTo != null)
                throw new Exception("Donation already assigned");

            donation.Quantity -= dto.Quantity;
            donation.AssignStatus = AssignStatus.AssigningApproved;
            donation.UserAssignedTo = dto.ReceiverUserId;

            donation.IsAvailable = donation.Quantity > 0;
            donation.IsOrderd = false;

            _equipmentRepo.Update(donation);
            await _equipmentRepo.SaveChanges();
        }

        public async Task ApproveAssignMedicineAsync(ApproveDonationDto dto)
        {
            var donation = await _medicineRepo.GetById(dto.DonationId);

            if (donation == null)
                throw new Exception("Donation not found");

            if (donation.Quantity < dto.Quantity)
                throw new Exception("Quantity exceeds available amount");

            if (donation.UserAssignedTo != null)
                throw new Exception("Donation already assigned");

            donation.Quantity -= dto.Quantity;
            donation.AssignStatus = AssignStatus.AssigningApproved;
            donation.UserAssignedTo = dto.ReceiverUserId;

            donation.IsAvailable = donation.Quantity > 0;
            donation.IsOrderd = false;

            _medicineRepo.Update(donation);
            await _medicineRepo.SaveChanges();
        }

        public async Task RejectEquipmentAsync(RequestRejectDonationDto dto)
        {
            var donation = await _equipmentRepo.GetById(dto.DonationId);
            if (donation == null) throw new Exception("Donation not found");

            donation.IsOrderd = false;
            donation.AssignStatus = AssignStatus.AssigningRejected;
            donation.UserAssignedTo = null;

            _equipmentRepo.Update(donation);
            await _equipmentRepo.SaveChanges();
        }

        public async Task RejectMedicineAsync(RequestRejectDonationDto dto)
        {
            var donation = await _medicineRepo.GetById(dto.DonationId);
            if (donation == null) throw new Exception("Donation not found");

            donation.IsOrderd = false;
            donation.AssignStatus = AssignStatus.AssigningRejected;
            donation.UserAssignedTo = null;

            _medicineRepo.Update(donation);
            await _medicineRepo.SaveChanges();
        }

        public async Task<int> GetAllDonationsAsync()
        {
            var equipmentCount = await _equipmentRepo.GetAll().CountAsync();
            var medicineCount = await _medicineRepo.GetAll().CountAsync();

            return equipmentCount + medicineCount;
        }

    }

}
