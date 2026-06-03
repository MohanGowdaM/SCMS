using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartClinic.Application.DTOs;
using SmartClinic.Application.Interfaces;

namespace SmartClinic.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ScanController : Controller
    {
        private readonly IScanService _scanService;

        public ScanController(IScanService scanService)
        {
            _scanService = scanService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetScanQueue")]
        public async Task<IActionResult> GetScanQueue()
        {
            var result =
                await _scanService
                    .GetScanQueueAsync();

            return Ok(new
            {
                success = true,

                data = result
            });
        }

        [HttpPost("CompleteScan")]
        public async Task<IActionResult> CompleteScan([FromBody] CompleteScanRequest request)
        {
            try
            {
                var result =
                    await _scanService
                        .CompleteScanAsync(
                            request.Id,
                            request.TokenId);

                if (!result.Success)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message =
                            result.Message
                    });
                }

                return Ok(new
                {
                    success = true,
                    message =
                        result.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message =
                        ex.Message
                });
            }
        }
    }
}
