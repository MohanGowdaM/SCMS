using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartClinic.Web.Filters;
using SmartClinic.Web.Models;
using SmartClinic.Web.Models.Appointment;
using SmartClinic.Web.Models.Patient;
using SmartClinic.Web.Models.Payment;
using SmartClinic.Web.Models.SystemAdmin;
using System.Net.Http.Headers;
using System.Text;

namespace SmartClinic.Web.Controllers
{
    [RoleAuthorize("Admin", "Doctor", "Receptionist")]
    public class AppointmentsController : Controller
    {
        private readonly HttpClient _httpClient;

        public AppointmentsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("SmartClinicAPI");
        }
        public async Task<IActionResult> GetAllAppointments()
        {
            return View();           
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointments()
        {
            var token = HttpContext.Session.GetString("JWToken");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);

            var response = await _httpClient.GetAsync("Appointment/GetAllAppointments");

            if (!response.IsSuccessStatusCode)
            {
                return Json(new
                {
                    success = false,
                    data = new List<object>()
                });
            }

            var result = await response.Content.ReadAsStringAsync();

            // Deserialize API response
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<AppointmentDto>>>(result);

            return Json(new
            {
                success = apiResponse?.Success ?? false,

                data = apiResponse?.Data
            });
        }

        [HttpGet]
        public IActionResult GenerateTokenModal()
        {
            return PartialView("_GenerateTokenModal");
        }

        [HttpGet]
        public async Task<IActionResult>SearchPatients(string search)
{
            try
            {
                // Get JWT Token
                var token = HttpContext.Session.GetString("JWToken");

                if (string.IsNullOrEmpty(token))
                {
                    return Json(
                        new List<PatientDto>());
                }

                // Add Token
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);

                // API Call
                var response = await _httpClient.GetAsync($"Patients/SearchPatients?search={search}");

                if (!response.IsSuccessStatusCode)
                {
                    return Json(new List<PatientDto>());
                }

                var result = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<PatientDto>>>(result);

                return Json(apiResponse?.Data ?? new List<PatientDto>());
            }
            catch
            {
                return Json(new List<PatientDto>());
            }
        }

        [HttpGet]
        public async Task<IActionResult>GetPatientDetails(int patientId)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWToken");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);

                var response = await _httpClient.GetAsync($"Patients/GetPatientDetails/{patientId}");

                if (!response.IsSuccessStatusCode)
                {
                    return Json(null);
                }

                var result =await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<ApiResponse<PatientDto>>(result);

                return Json(apiResponse?.Data);                
            }
            catch
            {
                return Json(null);
            }
        }

        [HttpGet]
        public async Task<IActionResult>GetDepartments()
        {
            var token =
                HttpContext.Session
                .GetString("JWToken");

            _httpClient
                .DefaultRequestHeaders
                .Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        token);

            var response =
                await _httpClient.GetAsync("SystemAdmin/Departments");

            var result = await response.Content.ReadAsStringAsync();            

            var departments = JsonConvert.DeserializeObject<List<DepartmentDto>>(result);

            return Json(departments);
        }

        [HttpGet]
        public async Task<IActionResult>GetDoctorsByDepartment(int departmentId)
        {
            var token =
                HttpContext.Session
                .GetString("JWToken");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);

            var response = await _httpClient.GetAsync($"SystemAdmin/GetDoctorsByDepartment/{departmentId}");

            if (!response.IsSuccessStatusCode)
            {
                return Json(new List<DoctorDto>());
            }
            var result = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<DoctorDto>>>(result);

            return Json(apiResponse?.Data ?? new List<DoctorDto>());
        }

        [HttpGet]
        public async Task<IActionResult>GetPriorities()
        {
            var token = HttpContext.Session.GetString("JWToken");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);

            var response = await _httpClient.GetAsync("SystemAdmin/GetPriorityConfigurations");

            var result = await response.Content.ReadAsStringAsync();

            var Priorities = JsonConvert.DeserializeObject<List<PriorityConfigDto>>(result);

            return Json(Priorities);
        }

        public async Task<IActionResult> GetSymptoms()
        {
            var token =
                HttpContext.Session
                .GetString("JWToken");

            _httpClient
                .DefaultRequestHeaders
                .Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var response =
                await _httpClient
                .GetAsync("SystemAdmin/GetSymptoms");

            if (!response.IsSuccessStatusCode)
            {
                return Json(new List<object>());
            }

            var result =
                await response.Content
                .ReadAsStringAsync();

            var symptoms =
    JsonConvert.DeserializeObject
    <List<SymptomDto>>
    (result);

            return Json(symptoms);
        }

        [HttpPost]
        public async Task<IActionResult>CalculatePriority(string symptoms)
        {
            var token = HttpContext.Session.GetString("JWToken");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);

            var response = await _httpClient.GetAsync($"SystemAdmin/CalculatePriority?symptoms={symptoms}");

            if (!response.IsSuccessStatusCode)
            {
                return Json(null);
            }

            var result = await response.Content.ReadAsStringAsync();

            return Content(result, "application/json");
        }

        [HttpPost]
        public async Task<IActionResult> GenerateToken([FromBody] GenerateTokenRequest request)
        {
            var token = HttpContext.Session.GetString("JWToken");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);

            var response = await _httpClient.PostAsJsonAsync($"SystemAdmin/GenerateToken",request);

            if (!response.IsSuccessStatusCode)
            {
                return Json(new
                {
                    success = false,
                    message =
                        "Unable to generate token"
                });
            }

            var result = await response.Content.ReadAsStringAsync();

            return Content(result,"application/json");
        }


        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int appointmentId,string status)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWToken");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);

                var request = new
                    {
                        AppointmentId = appointmentId,

                        Status = status
                    };

                var response = await _httpClient.PostAsJsonAsync("Appointment/UpdateStatus", request);

                if (!response.IsSuccessStatusCode)
                {
                    return Json(new
                    {
                        success = false,
                        message ="Unable to update appointment status"
                    });
                }

                var result = await response.Content.ReadAsStringAsync();

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


        [HttpPost]
        public async Task<IActionResult>CheckAppointmentConflict([FromBody] CheckAppointmentConflictRequest request)
        {
            var token = HttpContext.Session.GetString("JWToken");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);

            var response = await _httpClient.PostAsJsonAsync("Appointment/CheckAppointmentConflict",request);

            if (!response.IsSuccessStatusCode)
            {
                return Json(new
                {
                    success = false
                });
            }

            var result = await response.Content.ReadAsStringAsync();

            return Content(result,"application/json");
        }

        [HttpPost]
        public async Task<IActionResult> CollectPayment([FromBody] PaymentRequest request)
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
                if (request == null || request.PatientId <= 0 || request.ConsultationFee <= 0)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Invalid payment request data"
                    });
                }

                // Validate payment mode
                if (string.IsNullOrEmpty(request.PaymentMode))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Payment mode is required"
                    });
                }

                // Add JWT Token to Authorization Header
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Serialize request to JSON
                var json = JsonConvert.SerializeObject(request);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Call API to collect payment
                var response = await _httpClient.PostAsync("Appointment/CollectPayment", content);

                if (!response.IsSuccessStatusCode)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Failed to process payment"
                    });
                }

                // Read API response
                var result = await response.Content.ReadAsStringAsync();

                // Deserialize API response
                var apiResponse = JsonConvert.DeserializeObject<dynamic>(result);

                // Return the API response
                return Json(new
                {
                    success = true,
                    data = apiResponse,
                    message = "Payment processed successfully"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = $"Error processing payment: {ex.Message}"
                });
            }
        }

    }
}
