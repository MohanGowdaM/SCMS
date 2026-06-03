using Microsoft.AspNetCore.Mvc;
using SmartClinic.Web.Filters;
using SmartClinic.Web.Models.SystemAdmin;
using SmartClinic.Web.Services;

namespace SmartClinic.Web.Controllers
{
    [RoleAuthorize("Admin", "Doctor", "Receptionist")]
    public class SystemAdminController : Controller
    {
        #region  SystemAdmin Controller
        private readonly SystemAdminApiService _apiService;
        private readonly ILogger<SystemAdminController> _logger;
        public SystemAdminController(SystemAdminApiService apiService, ILogger<SystemAdminController> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }
        // Single page entry for system administration UI
        public IActionResult Index()
        {
            return View();
        }
        #region Roles
        [HttpGet]
        public async Task<IActionResult> RolesList()
        {
            try
            {
                var items = await _apiService.GetRolesAsync();
                return Json(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching roles");
                return StatusCode(500, "An error occurred while fetching roles.");
            }

        }
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleDto model)
        {
            try
            {
                if (model == null) return BadRequest();
                var ok = await _apiService.CreateRoleAsync(model);
                return Json(new { success = ok });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating role");
                return StatusCode(500, "An error occurred while creating the role.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateRole([FromBody] RoleDto model)
        {
            try
            {
                if (model == null) return BadRequest();
                var ok = await _apiService.UpdateRoleAsync(model.Id, model);
                return Json(new { success = ok });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating role");
                return StatusCode(500, "An error occurred while updating the role.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                var ok = await _apiService.DeleteRoleAsync(id);
                return Json(new { success = ok });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting role");
                return StatusCode(500, "An error occurred while deleting the role.");
            }

        }
        #endregion
        #region Users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var items = await _apiService.GetUsersAsync();
                return Json(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching users");
                return StatusCode(500, "An error occurred while fetching users.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDto model)
        {
            try
            {
                if (model == null) return BadRequest();
                var ok = await _apiService.CreateUserAsync(model);
                return Json(new { success = ok });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return StatusCode(500, "An error occurred while creating the user.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto model)
        {
            try
            {
                if (model == null) return BadRequest();
                var ok = await _apiService.UpdateUserAsync(model.Id, model);
                return Json(new { success = ok });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user");
                return StatusCode(500, "An error occurred while updating the user.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var ok = await _apiService.DeleteUserAsync(id);
                return Json(new { success = ok });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user");
                return StatusCode(500, "An error occurred while deleting the user.");
            }
        }
        #endregion
        #region Departments
        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            try
            {
                var items = await _apiService.GetDepartmentsAsync();
                return Json(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching departments");
                return StatusCode(500, "An error occurred while fetching departments.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentDto model)
        {
            try
            {
                if (model == null) return BadRequest();
                var ok = await _apiService.CreateDepartmentAsync(model);
                return Json(new { success = ok });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating department");
                return StatusCode(500, "An error occurred while creating the department.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateDepartment([FromBody] DepartmentDto model)
        {
            try
            {
                if (model == null) return BadRequest();
                var ok = await _apiService.UpdateDepartmentAsync(model.Id, model);
                return Json(new { success = ok });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating department");
                return StatusCode(500, "An error occurred while updating the department.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var ok = await _apiService.DeleteDepartmentAsync(id);
                return Json(new { success = ok });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting department");
                return StatusCode(500, "An error occurred while deleting the department.");
            }
        }
        #endregion
        #region Doctors
        [HttpGet]
        public async Task<IActionResult> GetDoctors()
        {
            try
            {
                var items = await _apiService.GetDoctorsAsync();
                return Json(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching doctors");
                return StatusCode(500, "An error occurred while fetching doctors.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateDoctor([FromBody] DoctorDto model)
        {
            try
            {
                if (model == null) return BadRequest();
                var ok = await _apiService.CreateDoctorAsync(model);
                return Json(new { success = ok });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating doctor");
                return StatusCode(500, "An error occurred while creating the doctor.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateDoctor([FromBody] DoctorDto model)
        {
            try
            {
                if (model == null) return BadRequest();
                var ok = await _apiService.UpdateDoctorAsync(model.Id, model);
                return Json(new { success = ok });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating doctor");
                return StatusCode(500, "An error occurred while updating the doctor.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            try
            {
                var ok = await _apiService.DeleteDoctorAsync(id);
                return Json(new { success = ok });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting doctor");
                return StatusCode(500, "An error occurred while deleting the doctor.");
            }
        }
        #endregion
        #region Symptoms
        [HttpGet]
        public async Task<IActionResult> GetSymptoms()
        {
            try
            {
                var items = await _apiService.GetSymptomsAsync();
                return Json(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching symptoms");
                return StatusCode(500, "An error occurred while fetching symptoms.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateSymptom([FromBody] SymptomDto model)
        {
            try
            {
                if (model == null) return BadRequest();
                var ok = await _apiService.CreateSymptomAsync(model);
                return Json(new { success = ok });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating symptom");
                return StatusCode(500, "An error occurred while creating the symptom.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSymptom([FromBody] SymptomDto model)
        {
            try
            {
                if (model == null) return BadRequest();
                var ok = await _apiService.UpdateSymptomAsync(model.Id, model);
                return Json(new { success = ok });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating symptom");
                return StatusCode(500, "An error occurred while updating the symptom.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSymptom(int id)
        {
            try
            {
                var ok = await _apiService.DeleteSymptomAsync(id);
                return Json(new { success = ok });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting symptom");
                return StatusCode(500, "An error occurred while deleting the symptom.");
            }
        }
        #endregion
        #region Priority Configurations
        [HttpGet]
        public async Task<IActionResult> GetPriorities()
        {
            try
            {
                var items = await _apiService.GetPriorityConfigsAsync();
                return Json(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching priority configurations");
                return StatusCode(500, "An error occurred while fetching priority configurations.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreatePriority([FromBody] PriorityConfigDto model)
        {
            try
            {
                if (model == null) return BadRequest();
                var ok = await _apiService.CreatePriorityConfigAsync(model);
                return Json(new { success = ok });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating priority configuration");
                return StatusCode(500, "An error occurred while creating the priority configuration.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePriority([FromBody] PriorityConfigDto model)
        {
            try
            {
                if (model == null) return BadRequest();
                var ok = await _apiService.UpdatePriorityConfigAsync(model.Id, model);
                return Json(new { success = ok });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating priority configuration");
                return StatusCode(500, "An error occurred while updating the priority configuration.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeletePriority(int id)
        {
            try
            {
                var ok = await _apiService.DeletePriorityConfigAsync(id);
                return Json(new { success = ok });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting priority configuration");
                return StatusCode(500, "An error occurred while deleting the priority configuration.");
            }
        }
        #endregion
        #region Token Statuses
        [HttpGet]
        public async Task<IActionResult> GetTokenStatuses()
        {
            try
            {
                var items = await _apiService.GetTokenStatusesAsync();
                return Json(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching token statuses");
                return StatusCode(500, "An error occurred while fetching token statuses.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateTokenStatus([FromBody] TokenStatusDto model)
        {
            try
            {
                if (model == null) return BadRequest();
                var ok = await _apiService.CreateTokenStatusAsync(model);
                return Json(new { success = ok });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating token status");
                return StatusCode(500, "An error occurred while creating the token status.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateTokenStatus([FromBody] TokenStatusDto model)
        {
            try
            {
                if (model == null) return BadRequest();
                var ok = await _apiService.UpdateTokenStatusAsync(model.Id, model);
                return Json(new { success = ok });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating token status");
                return StatusCode(500, "An error occurred while updating the token status.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteTokenStatus(int id)
        {
            try
            {
                var ok = await _apiService.DeleteTokenStatusAsync(id);
                return Json(new { success = ok });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting token status");
                return StatusCode(500, "An error occurred while deleting the token status.");
            }
        }
        #endregion
        #endregion

    }
}
