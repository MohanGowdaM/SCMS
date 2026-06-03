using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartClinic.Application.Interfaces;
using SmartClinic.Application.DTOs;

namespace SmartClinic.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : Controller
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }


        [HttpGet("GetDoctorQueue")]
        public async Task<IActionResult> GetDoctorQueue(int doctorId)
        {
            try
            {
                var result = await _doctorService.GetDoctorQueueAsync(doctorId);

                if (result == null)
                {
                    return BadRequest(
                        new
                        {
                            success = false,
                            message = "Doctor not found"
                        });
                }

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
             

        [HttpPost("UpdateQueueStatus")]
        public async Task<IActionResult> UpdateQueueStatus([FromBody] UpdateQueueRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid request data"
                    });
                }

                if (request.DoctorId <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Doctor Id is required"
                    });
                }

                // TokenId required for these actions
                if ((request.Action == "END" ||
                     request.Action == "HOLD" ||
                     request.Action == "SKIP" ||
                     request.Action == "RECALL" ||
                     request.Action == "NEXT")
                     &&
                     (!request.TokenId.HasValue || request.TokenId <= 0))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Token Id is required"
                    });
                }

                var result = await _doctorService.UpdateQueueStatusAsync(request);

                if (!result.Success)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = result.Message
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = result.Message,
                    data = result.Data
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


        [HttpPost("SaveConsultation")]
        public async Task<IActionResult>SaveConsultation([FromBody] SaveConsultationRequest request)
        {
            try
            {
                var result =
                    await _doctorService.SaveConsultationAsync(request);

                return Ok(
                    new
                    {
                        success =
                            result.Success,

                        message =
                            result.Message
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new
                    {
                        success = false,

                        message =
                            ex.Message
                    });
            }
        }

        [HttpPost("SendToLab")]
        public async Task<IActionResult>SendToLab([FromBody] SendToLabRequest request)
        {
            var result = await _doctorService.SendToLabAsync(request);

            return Ok(
                new
                {
                    success = result.Success,

                    message = result.Message
                });
        }

        [HttpPost("SendToScan")]
        public async Task<IActionResult> SendToScan([FromBody] SendToScanRequest request)
        {
            try
            {
                var result =
                    await _doctorService
                        .SendToScanAsync(request);

                if (!result.Success)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = result.Message
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = result.Message
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

        [HttpPost("SendToBilling")]
        public async Task<IActionResult> SendToBilling([FromBody] SendToBillingRequest request)
        {
            try
            {
                var result = await _doctorService.SendToBillingAsync(request);

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


        [HttpGet("GetMedicines")]
        public async Task<IActionResult>GetMedicines()
        {
            var result = await _doctorService.GetMedicinesAsync();

            return Ok(new
            {
                success = true,
                data = result
            });
        }

        [HttpPost("AddMedicine")]
        public async Task<IActionResult>AddMedicine([FromBody] AddMedicineRequest request)
        {
            var result = await _doctorService.AddMedicineAsync(request);

            return Ok(result);
        }


        [HttpGet("GetLabTests")]
        public async Task<IActionResult>GetLabTests()
        {
            try
            {
                var data = await _doctorService.GetLabTestsAsync();

                return Ok(
                    new
                    {
                        success = true,

                        data = data
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new
                    {
                        success = false,

                        message = ex.Message
                    });
            }
        }

        [HttpGet("GetScanTests")]
        public async Task<IActionResult>GetScanTests()
        {
            var result = await _doctorService.GetScanTestsAsync();

            return Ok(result);
        }
    }
}
