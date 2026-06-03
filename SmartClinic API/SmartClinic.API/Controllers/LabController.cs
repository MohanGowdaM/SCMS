using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartClinic.Application.DTOs;
using SmartClinic.Application.Interfaces;

namespace SmartClinic.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LabController : Controller
    {
        private readonly ILabService _labService;

        public LabController(ILabService labService)
        {
            _labService = labService;
        }

        [HttpGet("GetLabQueue")]
        public async Task<IActionResult> GetLabQueue()
        {
            var result = await _labService.GetLabQueueAsync();

            return Ok(
                new
                {
                    success = true,

                    data = result
                });
        }

        [HttpPost("CompleteLabTest")]
        public async Task<IActionResult> CompleteLabTest([FromBody] CompleteLabTestRequest request)
        {
            try
            {
                // Validate request
                if (request == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Request body cannot be empty"
                    });
                }

                if (request.Id <= 0 || request.TokenId <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid lab request ID or token ID"
                    });
                }

                // Call service to complete lab test
                var result = await _labService.CompleteLabTestAsync(request);

                if (!result.Success)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = result.Message
                    });
                }

                // Return success response with updated queue
                return Ok(new
                {
                    success = true,
                    message = result.Message,
                    data = result.Data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = $"Error completing lab test: {ex.Message}"
                });
            }
        }
    }
}
