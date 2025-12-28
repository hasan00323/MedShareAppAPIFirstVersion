using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedShareAppAPI.Controllers
{
    [ApiController]
    [Route("api/donations")]
    public class DonationController : ControllerBase
    {
        private readonly IDonationService _donationService;

        public DonationController(IDonationService service)
        {
            _donationService = service;
        }

        [Authorize(Roles = "User")]
        [HttpPost("{donationId}/requestEquipment")]
        public async Task<IActionResult> RequestDonation(int donationId)
        {
            await _donationService.RequestEquipmentAsync(donationId);
            return Ok("Donation requested");
        }

        [Authorize(Roles = "User")]
        [HttpPost("{donationId}/requestMedicine")]
        public async Task<IActionResult> RequestMedicineDonation(int donationId)
        {
            await _donationService.RequestMedicineAsync(donationId);
            return Ok("Donation requested");
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("approveEquipment/{donationId}/quantity/{quantity}/user/{userId}")]
        public async Task<IActionResult> ApproveAssignEquipment(int donationId, int quantity, int userId)
        {
            await _donationService.ApproveAssignEquipmentAsync(donationId, quantity, userId);
            return Ok("Donation approved");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("approveMedicine/{donationId}/quantity/{quantity}/user/{userId}")]
        public async Task<IActionResult> ApproveAssignMedicine(int donationId, int quantity, int userId)
        {
            await _donationService.ApproveAssignMedicineAsync(donationId, quantity, userId);
            return Ok("Donation approved");
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("{donationId}/rejectEquipment")]
        public async Task<IActionResult> RejectEquipmentDonation(int donationId)
        {
            await _donationService.RejectEquipmentAsync(donationId);
            return Ok("Donation rejected");
        }
    
    
        [Authorize(Roles = "Admin")]
        [HttpPut("{donationId}/rejectMedicine")]
        public async Task<IActionResult> RejectMedicineDonation(int donationId)
        {
            await _donationService.RejectMedicineAsync(donationId);
            return Ok("Donation rejected");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("allDonations")]
        public async Task<IActionResult> GetAllDonations()
        {
            return Ok(await _donationService.GetAllDonationsAsync());
        }

    }

}
