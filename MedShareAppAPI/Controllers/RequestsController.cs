using Application.DTOs.AllRequests.Requests;
using Application.DTOs.AllRequests.Requests.Carts;
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
        [HttpPost("createEquipmentRequest")]
        public async Task<IActionResult> CreateEquipmentRequest([FromBody] RequestUploadEquipmentDto dto)
        {
            await _requestService.CreateEquipmentRequestAsync(dto);
            return Ok("Equipment request created successfully");
        }

        [Authorize]
        [HttpPost("createMedicineRequest")]
        public async Task<IActionResult> CreateMedicineRequest([FromBody] RequestUploadMedicineDto dto)
        {
            await _requestService.CreateMedicineRequestAsync(dto);
            return Ok("Medicine request created successfully");
        }

       
        [Authorize(Roles = "Admin")]
        [HttpGet("getPendingEquipmentRequests")]
        public async Task<IActionResult> GetPendingEquipmentRequests()
        {
            return Ok(await _requestService.GetPendingEquipmentRequests());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getPendingMedicineRequests")]
        public async Task<IActionResult> GetPendingMedicineRequests()
        {
            return Ok(await _requestService.GetPendingMedicineRequests());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getUnavailableEquipmentRequests")]
        public async Task<IActionResult> GetUnavailableEquipmentRequests()
        {
            return Ok(await _requestService.GetUnavailableEquipmentRequests());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getUnavailableMedicineRequests")]
        public async Task<IActionResult> GetUnavailableMedicineRequests()
        {
            return Ok(await _requestService.GetUnavailableMedicineRequests());
        }

     
        [Authorize(Roles = "Admin")]
        [HttpPut("approveEquipmentRequest")]
        public async Task<IActionResult> ApproveEquipmentRequest([FromBody] RequestDto dto)
        {
            await _requestService.ApproveEquipmentRequestAsync(dto);
            return Ok("Request approved and donation created");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("approveMedicineRequest")]
        public async Task<IActionResult> ApproveMedicineRequest([FromBody] RequestDto dto)
        {
            await _requestService.ApproveMedicineRequestAsync(dto);
            return Ok("Request approved and donation created");
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("rejectEquipmentRequest")]
        public async Task<IActionResult> RejectEquipmentRequest([FromBody] RequestDto dto)
        {
            await _requestService.RejectEquipmentRequestAsync(dto);
            return Ok("Request rejected");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("rejectMedicineRequest")]
        public async Task<IActionResult> RejectMedicineRequest([FromBody] RequestDto dto)
        {
            await _requestService.RejectMedicineRequestAsync(dto);
            return Ok("Request rejected");
        }

        [Authorize(Roles = "User")]
        [HttpPut("addEquipmentToCart")]
        public async Task<IActionResult> AddEquipmentToCart([FromBody] AddToCartDto dto)
        {
            await _requestService.AddEquipmentToCartAsync(dto);
            return Ok("Equipment added to cart");
        }

        [Authorize(Roles = "User")]
        [HttpPut("addMedicineToCart")]
        public async Task<IActionResult> AddMedicineToCart([FromBody] AddToCartDto dto)
        {
            await _requestService.AddMedicineToCartAsync(dto);
            return Ok("Medicine added to cart");
        }

        [Authorize(Roles = "User")]
        [HttpPut("removeEquipmentFromCart")]
        public async Task<IActionResult> RemoveEquipmentFromCart(
            [FromBody] AddToCartDto dto)
        {
            await _requestService.RemoveEquipmentFromCartAsync(dto);
            return Ok("Equipment removed from cart");
        }

        [Authorize(Roles = "User")]
        [HttpPut("removeMedicineFromCart")]
        public async Task<IActionResult> RemoveMedicineFromCart(
            [FromBody] AddToCartDto dto)
        {
            await _requestService.RemoveMedicineFromCartAsync(dto);
            return Ok("Medicine removed from cart");
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("getAllUnavailableDonationRequests")]
        public async Task<IActionResult> AllUnavailableDonationRequests()
        {
            return Ok(await _requestService.AllUnavailableDonationRequests());
        }
    }

}
