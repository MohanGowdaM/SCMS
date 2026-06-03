using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartClinic.Web.Models;
using SmartClinic.Web.Models.Lab;
using System.Text;

namespace SmartClinic.Web.Controllers
{
    public class LabController : Controller
    {
        private readonly HttpClient _httpClient;

        public LabController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("SmartClinicAPI");
        }
        public async Task<IActionResult> LabQueue()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> GetLabQueue()
        {
            try
            {
                // Get JWT Token
                var token =
                    HttpContext.Session
                        .GetString(
                            "JWToken"
                        );

                if (string.IsNullOrEmpty(token))
                {
                    return Json(new
                    {
                        success = false,
                        message =
                            "Session expired. Please login again."
                    });
                }

                // Add JWT Token
                _httpClient
                    .DefaultRequestHeaders
                    .Authorization =
                        new System.Net.Http.Headers
                            .AuthenticationHeaderValue(
                                "Bearer",
                                token);

                // Call API
                var response = await _httpClient.GetAsync("Lab/GetLabQueue");

                if (!response.IsSuccessStatusCode)
                {
                    return Json(new
                    {
                        success = false,
                        message =
                            "Failed to load lab queue"
                    });
                }

                // Read API Response
                var result =
                    await response.Content
                        .ReadAsStringAsync();

                // Return raw JSON
                return Content(
                    result,
                    "application/json"
                );
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message =
                        ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CompleteLabTest([FromBody] CompleteLabTestRequest request)
        {
            try
            {
                // Get JWT Token from Session
                var token = HttpContext.Session.GetString("JWToken");

                if (string.IsNullOrEmpty(token))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Session expired. Please login again."
                    });
                }

                // Validate request data
                if (request == null || request.Id <= 0 || request.TokenId <= 0)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Invalid request data"
                    });
                }

                // Add JWT Token to Authorization Header
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Serialize request to JSON
                var json = JsonConvert.SerializeObject(request);

                var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

                // Call API
                var response = await _httpClient.PostAsync(
                    "Lab/CompleteLabTest",
                    content);

                if (!response.IsSuccessStatusCode)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Failed to complete lab test"
                    });
                }

                // Read API response
                var result = await response.Content.ReadAsStringAsync();

                // Return API response
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}
