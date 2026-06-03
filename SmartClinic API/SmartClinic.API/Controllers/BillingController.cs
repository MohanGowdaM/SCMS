using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartClinic.Application.DTOs;
using SmartClinic.Application.Interfaces;

namespace SmartClinic.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BillingController : Controller
    {
        private readonly IBillingService _billingService;

        public BillingController(IBillingService billingService)
        {
            _billingService = billingService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetBillingQueue")]
        public async Task<IActionResult> GetBillingQueue()
        {
            try
            {
                var result = await _billingService.GetBillingQueueAsync();

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

        [HttpPost("CompletePayment")]
        public async Task<IActionResult>CompletePayment([FromBody] CompletePaymentRequest request)
        {
            var result = await _billingService.CompletePaymentAsync(request);

            return Ok(new
            {
                success = result.Success,

                message = result.Message
            });
        }

        [HttpPost("SendToPharmacy")]
        public async Task<IActionResult>SendToPharmacy([FromBody] SendToPharmacyRequest request)
        {
            var result =
                await _billingService
                    .SendToPharmacyAsync(
                        request.TokenId
                    );

            return Ok(new
            {
                success =
                    result.Success,

                message =
                    result.Message
            });
        }

        [HttpPost("CompleteVisit")]
        public async Task<IActionResult> CompleteVisit([FromBody] SendToPharmacyRequest request)
        {
            var result =
                await _billingService
                    .CompleteVisitAsync(
                        request.TokenId
                    );

            return Ok(new
            {
                success =
                    result.Success,

                message =
                    result.Message
            });
        }
    }
}
