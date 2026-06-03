using Microsoft.AspNetCore.Mvc;
using SmartClinic.Web.Models.Admission;
using System.Net.Http.Headers;

namespace SmartClinic.Web.Controllers
{
    public class AdmissionController : Controller
    {
        private readonly HttpClient _httpClient;

        public AdmissionController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("SmartClinicAPI");
        }
        public IActionResult AdmitQueue()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult>GetAdmitQueue()
        {
            try
            {
                var token = HttpContext.Session.GetString("JWToken");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);

                var response = await _httpClient.GetAsync("Admission/GetAdmitQueue");

                var json = await response.Content.ReadAsStringAsync();

                return Content(json,"application/json");
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

        [HttpPost]
        public async Task<IActionResult>CompleteAdmission([FromBody] CompleteAdmissionRequest request)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWToken");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);

                var response = await _httpClient.PostAsJsonAsync("Admission/CompleteAdmission",request);

                var json = await response.Content.ReadAsStringAsync();

                return Content(json,"application/json");
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
