using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartClinic.Web.Models.Scan;
using System.Text;

namespace SmartClinic.Web.Controllers
{
    public class ScanController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ScanController> _logger;
        public ScanController(IHttpClientFactory httpClientFactory, ILogger<ScanController> logger)
        {
            _httpClient = httpClientFactory.CreateClient("SmartClinicAPI");
            _logger = logger;
        }
        public IActionResult ScanQueue()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetScanQueue()
        {
            try
            {
                var token = HttpContext.Session.GetString("JWToken");
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { success = false, message = "Session expired" });
                }
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync("Scan/GetScanQueue");
                if (!response.IsSuccessStatusCode)
                {
                    return Json(new { success = false, message = "Failed to load scan queue" });
                }
                var result = await response.Content.ReadAsStringAsync();
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> CompleteScan([FromBody] CompleteScanRequest request)
        {
            try
            {
                // Get JWT Token from Session
                var token = HttpContext.Session.GetString("JWToken");
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { success = false, message = "Session expired. Please login again." });
                }
                // Validate Request
                if (request == null || request.Id <= 0 || request.TokenId <= 0)
                {
                    return Json(new { success = false, message = "Invalid scan request data" });
                }
                // Add JWT Token
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                // Serialize Request
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                // Call API
                var response = await _httpClient.PostAsync("Scan/CompleteScan", content);

                if (!response.IsSuccessStatusCode)
                {
                    return Json(new { success = false, message = "Failed to complete scan" });
                }
                // Read API Response
                var result = await response.Content.ReadAsStringAsync();
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateScanTokenStatus([FromBody] UpdateTokenStatusDto model)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWToken");
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { success = false, message = "Session expired. Please login again." });
                }
                if (model == null || model.TokenId <= 0 || string.IsNullOrEmpty(model.Action))
                {
                    return Json(new { success = false, message = "Invalid request data" });
                }
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsJsonAsync("Scan/UpdateScanTokenStatus", model);
                if (!response.IsSuccessStatusCode)
                {
                    return Json(new { success = false, message = "Failed to update scan token status" });
                }
                else
                {
                    return Json(new { success = true, message = "Scan token status updated successfully" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
