using Microsoft.AspNetCore.Mvc;
using SmartClinic.Web.Models.Account;
using SmartClinic.Web.Services;

namespace SmartClinic.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthApiService _authApiService;

        public AccountController(AuthApiService authApiService)
        {
            _authApiService =
                authApiService;
        }
        
        
        [HttpGet]
        public IActionResult Login()
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (!string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index","Dashboard");
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult>Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response =await _authApiService.LoginAsync(model);

            if (response == null || !response.IsSuccess)
            {
                ViewBag.Error ="Invalid credentials";

                return View(model);
            }

            HttpContext.Session.SetString("JWToken",response.Token);

            HttpContext.Session.SetString("Username", response.Username);

            HttpContext.Session.SetString("Role",response.Role);

            HttpContext.Session.SetString("DoctorId",response.DoctorId.ToString() ?? "");

            // Role based redirect
            switch (response.Role)
            {
                case "Admin":

                    return RedirectToAction("Index","Dashboard");

                case "Doctor":

                    return RedirectToAction("GetAllPatientList","Patients");

                case "Receptionist":

                    return RedirectToAction("GetAllPatientList","Patients");

                default:

                    return RedirectToAction("AccessDenied","Account");
            }
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Logout()
        {
            // Clear all session
            HttpContext.Session.Clear();

            return RedirectToAction("Login","Account");
        }
    }
}
