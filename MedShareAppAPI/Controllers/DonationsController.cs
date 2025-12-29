using Application.DTOs.AllRequests.Donations;
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

        public DonationController(IDonationService donationService)
        {
            _donationService = donationService;
        }


        [Authorize(Roles = "User")]
        [HttpPost("requestEquipment")]
        public async Task<IActionResult> RequestEquipment([FromBody] RequestRejectDonationDto dto)
        {
            await _donationService.RequestEquipmentAsync(dto);
            return Ok("Donation requested");
        }

        [Authorize(Roles = "User")]
        [HttpPost("requestMedicine")]
        public async Task<IActionResult> RequestMedicine([FromBody] RequestRejectDonationDto dto)
        {
            await _donationService.RequestMedicineAsync(dto);
            return Ok("Donation requested");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("approveEquipment")]
        public async Task<IActionResult> ApproveEquipment([FromBody] ApproveDonationDto dto)
        {
            await _donationService.ApproveAssignEquipmentAsync(dto);
            return Ok("Donation approved");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("approveMedicine")]
        public async Task<IActionResult> ApproveMedicine([FromBody] ApproveDonationDto dto)
        {
            await _donationService.ApproveAssignMedicineAsync(dto);
            return Ok("Donation approved");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("rejectEquipment")]
        public async Task<IActionResult> RejectEquipment([FromBody] RequestRejectDonationDto dto)
        {
            await _donationService.RejectEquipmentAsync(dto);
            return Ok("Donation rejected");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("rejectMedicine")]
        public async Task<IActionResult> RejectMedicine([FromBody] RequestRejectDonationDto dto)
        {
            await _donationService.RejectMedicineAsync(dto);
            return Ok("Donation rejected");
        }

 
        [Authorize(Roles = "Admin")]
        [HttpGet("getAllDonations")]
        public async Task<IActionResult> GetAllDonations()
        {
            return Ok(await _donationService.GetAllDonationsAsync());
        }
    }


}
