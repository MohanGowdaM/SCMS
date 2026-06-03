using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartClinic.Application.DTOs;
using SmartClinic.Application.Interfaces;

namespace SmartClinic.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdmissionController : Controller
    {
        private readonly IAdmissionService _admissionService;

        public AdmissionController(IAdmissionService admissionService)
        {
            _admissionService = admissionService;
        }

        [HttpGet("GetAdmitQueue")]
        public async Task<IActionResult>GetAdmitQueue()
        {
            var result = await _admissionService.GetAdmitQueueAsync();

            return Ok(new
            {
                success = true,
                data = result
            });
        }

        [HttpPost("CompleteAdmission")]
        public async Task<IActionResult>CompleteAdmission([FromBody] CompleteAdmissionRequest request)
        {
            var result = await _admissionService.CompleteAdmissionAsync(request);

            return Ok(result);
        }
    }
}
