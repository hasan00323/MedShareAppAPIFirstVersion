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
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingRequests()
        {
            return Ok(await _requestService.GetPendingRequestsForAdmin());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("unavailable")]
        public async Task<IActionResult> GetUnAvailableRequests()
        {
            return Ok( await _requestService.GetUnAvailableRequests());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("rejected")]
        public async Task<IActionResult> GetRejectedRequests()
        {
            return Ok(  await _requestService.GetRejectedRequests());
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{requestId}/approve")]
        public async Task<IActionResult> ApproveRequest(int requestId)
        {
            await _requestService.ApproveRequestAsync(requestId);
            return Ok("Request approved and donation created");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{requestId}/reject")]
        public async Task<IActionResult> RejectRequest(int requestId)
        {
            await _requestService.RejectRequestAsync(requestId);
            return Ok("Request rejected");
        }
        [Authorize]
        [HttpGet("AllAprovedRequest")]
        public async Task<IActionResult> AllAprovedRequest()
        {
            return Ok(await _requestService.GatAllAprovedRequest());
        }
    }
}
