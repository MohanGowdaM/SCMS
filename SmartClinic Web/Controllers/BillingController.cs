using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartClinic.Web.Models.Billing;
using SmartClinic.Web.Models.Doctor;
using System.Text;

namespace SmartClinic.Web.Controllers
{
    public class BillingController : Controller
    {
        private readonly HttpClient _httpClient;

        public BillingController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("SmartClinicAPI");
        }
        public IActionResult BillingQueue()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetBillingQueue()
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

                _httpClient.DefaultRequestHeaders.Authorization =  new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync("Billing/GetBillingQueue");

                if (!response.IsSuccessStatusCode)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Failed to load billing queue"
                    });
                }

                var result = await response.Content.ReadAsStringAsync();

                return Content(result,"application/json");
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
        public async Task<IActionResult> CompletePayment([FromBody] CompletePaymentRequest request)
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

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",token);

                var json = JsonConvert.SerializeObject(request);

                var content = new StringContent(json,Encoding.UTF8,"application/json");

                var response = await _httpClient.PostAsync("Billing/CompletePayment",content);

                var result = await response.Content.ReadAsStringAsync();

                return Content(result,"application/json");
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
        public async Task<IActionResult>SendToPharmacy([FromBody] SendToPharmacyRequest request)
        {
            try
            {
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
                            "Session expired"
                    });
                }

                _httpClient
                    .DefaultRequestHeaders
                    .Authorization =
                        new System.Net.Http.Headers
                            .AuthenticationHeaderValue(
                                "Bearer",
                                token);

                var response =
                    await _httpClient
                        .PostAsJsonAsync(
                            "Billing/SendToPharmacy",
                            request);

                var result =
                    await response
                        .Content
                        .ReadAsStringAsync();

                return Content(
                    result,
                    "application/json");
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
        public async Task<IActionResult>CompleteVisit([FromBody] SendToPharmacyRequest request)
        {
            try
            {
                var token =
                    HttpContext.Session
                        .GetString(
                            "JWToken"
                        );

                _httpClient
                    .DefaultRequestHeaders
                    .Authorization =
                        new System.Net.Http.Headers
                            .AuthenticationHeaderValue(
                                "Bearer",
                                token);

                var response =
                    await _httpClient
                        .PostAsJsonAsync(
                            "Billing/CompleteVisit",
                            request);

                var result =
                    await response
                        .Content
                        .ReadAsStringAsync();

                return Content(
                    result,
                    "application/json");
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
    }
}
