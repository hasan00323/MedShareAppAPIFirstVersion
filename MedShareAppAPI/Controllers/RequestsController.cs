using Application.DTOs.Auth.Requests.RequestEquipment;
using Application.DTOs.Auth.Requests.RequestMedicine;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedShareAppAPI.Controllers
{
    [ApiController]
    [Route("api/requests")]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [Authorize]
        [HttpPost("equipment")]
        public async Task<IActionResult> CreateEquipmentRequest([FromBody] RequestEquipmentDto dto)
        {
            await _requestService.CreateEquipmentRequestAsync(dto);
            return Ok("Equipment request created successfully");
        }

        [Authorize]
        [HttpPost("medicine")]
        public async Task<IActionResult> CreateMedicineRequest([FromBody] RequestMedicineDto dto)
        {
            await _requestService.CreateMedicineRequestAsync(dto);
            return Ok("Medicine request created successfully");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("pendingEquipment")]
        public async Task<IActionResult> GetPendingEquipmentRequests()
        {
            return Ok(await _requestService.GetPendingEquipmentRequests());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("pendingMedicine")]
        public async Task<IActionResult> GetPendingMedicineRequests()
        {
            return Ok(await _requestService.GetPendingMedicineRequests());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("unavailableEquipment")]
        public async Task<IActionResult> GetUnAvailableEquipmentRequests()
        {
            return Ok( await _requestService.GetUnavailableEquipmentRequests());
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("unavailableMedicine")]
        public async Task<IActionResult> GetUnAvailableMedicineRequests()
        {
            return Ok( await _requestService.GetUnavailableMedicineRequests());
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("{requestId}/approveEquipment")]
        public async Task<IActionResult> ApproveEquipmentRequest(int requestId)
        {
            await _requestService.ApproveEquipmentRequestAsync(requestId);
            return Ok("Request approved and donation created");
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("{requestId}/approveMedicine")]
        public async Task<IActionResult> ApproveMedicineRequest(int requestId)
        {
            await _requestService.ApproveMedicineRequestAsync(requestId);
            return Ok("Request approved and donation created");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{requestId}/rejectEquipment")]
        public async Task<IActionResult> RejectEquipmentRequest(int requestId)
        {
            await _requestService.RejectEquipmentRequestAsync(requestId);
            return Ok("Request rejected");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{requestId}/rejectMedicine")]
        public async Task<IActionResult> RejectMedicineRequest(int requestId)
        {
            await _requestService.RejectMedicineRequestAsync(requestId);
            return Ok("Request rejected");
        }

        [Authorize(Roles = "User")]
        [HttpPut("{donationEquipmentId}/addEquipmentToCart")]
        public async Task<IActionResult> AddEquipmentToCart(int donationEquipmentId)
        {
            await _requestService.AddEquipmentToCartAsync(donationEquipmentId);
            return Ok("Equipment added to cart");
        }
        
        [Authorize(Roles = "User")]
        [HttpPut("{donationMedicineId}/addMedicineToCart")]
        public async Task<IActionResult> AddMedicineToCart(int donationMedicineId)
        {
            await _requestService.AddMedicineToCartAsync(donationMedicineId);
            return Ok("Medicine added to cart");
        }
        [Authorize(Roles = "User")]
        [HttpPut("{donationEquipmentId}/removeEquipmentFromCart")]
        public async Task<IActionResult> RemoveEquipmentFromCart(int donationEquipmentId)
        {
            await _requestService.RemoveEquipmentFromCartAsync(donationEquipmentId);
            return Ok("Equipment removed from cart");
        }

        [Authorize(Roles = "User")]
        [HttpPut("{donationMedicineId}/removeMedicineFromCart")]
        public async Task<IActionResult> RemoveMedicineFromCart(int donationMedicineId)
        {
            await _requestService.RemoveMedicineFromCartAsync(donationMedicineId);
            return Ok("Medicine removed from cart");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("allUnavailableDonationRequests")]
        public async Task<IActionResult> AllUnavailableDonationRequests()
        {
            var count = await _requestService.AllUnavailableDonationRequests();
            return Ok(count);
        }
    }
}
