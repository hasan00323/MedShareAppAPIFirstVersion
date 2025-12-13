using Application.DTOs.Auth.Requests.GetDonation;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [Authorize]
        [HttpPost("{donationId}/request")]
        public async Task<IActionResult> RequestDonation(GetDonationRequestDto donationRequest)
        {
            await _donationService.RequestDonationAsync(donationRequest.DonationId, donationRequest.UserId);
            return Ok("Donation requested");
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("{donationId}/approve")]
        public async Task<IActionResult> ApproveDonation(GetDonationRequestDto requested)
        {
            await _donationService.ApproveDonationRequestAsync(requested.DonationId,requested.Quantity);
            return Ok("Donation approved");
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("{donationId}/reject")]
        public async Task<IActionResult> RejectDonation(int donationId)
        {
            await _donationService.RejectDonationRequestAsync(donationId);
            return Ok("Donation rejected");
        }
    }

}
