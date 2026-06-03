using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartClinic.Application.DTOs;
using SmartClinic.Application.Interfaces;
using System.Net.Http.Headers;

namespace SmartClinic.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PharmacyController : Controller
    {
        private readonly IPharmacyService _pharmacyService;

        public PharmacyController(IPharmacyService pharmacyService)
        {
            _pharmacyService = pharmacyService;
        }        

        [HttpGet("GetPharmacyQueue")]
        public async Task<IActionResult>GetPharmacyQueue()
        {
            try
            {
                var result =await _pharmacyService.GetPharmacyQueueAsync();

                return Ok(new
                {
                    success = true,
                    data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }


        [HttpPost("DispenseMedicine")]
        public async Task<IActionResult>DispenseMedicine([FromBody] DispenseMedicineRequest request)
        {
            try
            {
                var result =
                    await _pharmacyService
                        .DispenseMedicineAsync(
                            request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}
