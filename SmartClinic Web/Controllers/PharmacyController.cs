using Microsoft.AspNetCore.Mvc;
using SmartClinic.Web.Models;
using SmartClinic.Web.Models.Pharmacy;
using System.Net.Http.Headers;

namespace SmartClinic.Web.Controllers
{
    public class PharmacyController : Controller
    {
        private readonly HttpClient _httpClient;

        public PharmacyController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("SmartClinicAPI");
        }

        public IActionResult PharmacyQueue()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetPharmacyQueue()
        {
            try
            {
                var token = HttpContext.Session.GetString("JWToken");

                if (string.IsNullOrEmpty(token))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Session expired"
                    });
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);

                var response = await _httpClient.GetAsync("Pharmacy/GetPharmacyQueue");

                if (!response.IsSuccessStatusCode)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Unable to fetch pharmacy queue"
                    });
                }

                var apiResponse =
                    await response.Content.ReadFromJsonAsync<dynamic>();

                return Json(apiResponse);
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
        public async Task<IActionResult> DispenseMedicine([FromBody] DispenseMedicineRequest request)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWToken");

                if (string.IsNullOrEmpty(token))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Session expired"
                    });
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);

                var response = await _httpClient.PostAsJsonAsync("Pharmacy/DispenseMedicine",request);

                if (!response.IsSuccessStatusCode)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Unable to connect API"
                    });
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();

                return Content(jsonResponse,"application/json");
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
