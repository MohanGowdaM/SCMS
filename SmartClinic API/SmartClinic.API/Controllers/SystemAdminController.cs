using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartClinic.Application.DTOs;
using SmartClinic.Application.Interfaces;
using SmartClinic.Domain.Entities;
namespace SmartClinic.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SystemAdminController : ControllerBase
    {
        #region SystemAdmin Controller
        private readonly ISystemAdminService _service;
        private readonly ILogger<SystemAdminController> _logger;
        public SystemAdminController(ISystemAdminService service, ILogger<SystemAdminController> logger)
        {
            _service = service;
            _logger = logger;
        }
        #region Roles
        [HttpGet("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var resp = await _service.GetRoles();
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching roles.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("GetRoleById/{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            try
            {
                var resp = await _service.GetRoleById(id);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching role by ID.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(RolesDto model)
        {
            try
            {
                var resp = await _service.CreateRole(model);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating role.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPut("UpdateRole")]
        public async Task<IActionResult> UpdateRole(RolesDto model)
        {
            try
            {
                var resp = await _service.UpdateRole(model);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating role.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpDelete("DeleteRole/{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                var resp = await _service.DeleteRole(id);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting role.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        #endregion
        #region Users
        [HttpGet("Users")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var resp = await _service.GetUsers();
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching users.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var resp = await _service.GetUserById(id);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user by ID.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPost("CreateUsers")]
        public async Task<IActionResult> CreateUser(UserDto model)
        {
            try
            {
                var resp = await _service.CreateUser(model);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating user.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserDto model)
        {
            try
            {
                var resp = await _service.UpdateUser(model);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var resp = await _service.DeleteUser(id);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        #endregion
        #region Departments
        [HttpGet("Departments")]
        public async Task<IActionResult> GetDepartments()
        {
            try
            {
                var resp = await _service.GetDepartments();
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching departments.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("Departments/{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            try
            {
                var resp = await _service.GetDepartmentById(id);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching department by ID.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPost("Departments")]
        public async Task<IActionResult> CreateDepartment(DepartmentDto model)
        {
            try
            {
                var resp = await _service.CreateDepartment(model);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating department.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPut("Departments/{id}")]
        public async Task<IActionResult> UpdateDepartment(DepartmentDto model)
        {
            try
            {
                var resp = await _service.UpdateDepartment(model);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating department.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpDelete("Departments/{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var resp = await _service.DeleteDepartment(id);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting department.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        #endregion
        #region Doctors
        [HttpGet("GetDoctors")]
        public async Task<IActionResult> GetDoctors()
        {
            try
            {
                var resp = await _service.GetDoctors();
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching doctors.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("GetDoctorById/{id}")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            try
            {
                var resp = await _service.GetDoctorById(id);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching doctor by ID.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPost("CreateDoctor")]
        public async Task<IActionResult> CreateDoctor(DoctorDto model)
        {
            try
            {
                var resp = await _service.CreateDoctor(model);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating doctor.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPut("UpdateDoctor")]
        public async Task<IActionResult> UpdateDoctor(DoctorDto model)
        {
            try
            {
                var resp = await _service.UpdateDoctor(model);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating doctor.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpDelete("DeleteDoctor/{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            try
            {
                var resp = await _service.DeleteDoctor(id);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting doctor.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("GetDoctorsByDepartment/{id}")]
        public async Task<IActionResult> GetDoctorsByDepartment(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { Message = "Invalid department ID. Department ID must be greater than 0.", Success = false });
                }
                var doctors = await _service.GetDoctorsByDepartmentAsync(id);
                if (doctors == null || doctors.Count == 0)
                {
                    return Ok(new { Message = $"No available doctors found for department ID: {id}", Success = true, Data = new List<DoctorDto>() });
                }
                return Ok(new { Message = $"Successfully retrieved {doctors.Count} doctor(s) for department ID: {id}", Success = true, Data = doctors });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { Message = $"Argument error while fetching doctors: {ex.Message}", Success = false, ErrorCode = "ARG_NULL_EXCEPTION" });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { Message = $"Invalid operation while fetching doctors: {ex.Message}", Success = false, ErrorCode = "INVALID_OPERATION" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred while retrieving doctors for the department.", Success = false, ErrorCode = "INTERNAL_SERVER_ERROR", Details = ex.Message });
            }
        }

        #endregion
        #region Symptoms
        [HttpGet("GetSymptoms")]
        public async Task<IActionResult> GetSymptoms()
        {
            try
            {
                var resp = await _service.GetSymptoms();
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching symptoms.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("GetSymptomById/{id}")]
        public async Task<IActionResult> GetSymptomById(int id)
        {
            try
            {
                var resp = await _service.GetSymptomById(id);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching symptom by ID.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPost("CreateSymptom")]
        public async Task<IActionResult> CreateSymptom(SymptomsDto model)
        {
            try
            {
                var resp = await _service.CreateSymptom(model);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating symptom.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPut("UpdateSymptom")]
        public async Task<IActionResult> UpdateSymptom(SymptomsDto model)
        {
            try
            {
                var resp = await _service.UpdateSymptom(model);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating symptom.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpDelete("DeleteSymptom/{id}")]
        public async Task<IActionResult> DeleteSymptom(int id)
        {
            try
            {
                var resp = await _service.DeleteSymptom(id);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting symptom.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        #endregion        
        #region Priority  Configuration
        [HttpGet("GetPriorityConfigurations")]
        public async Task<IActionResult> GetPriorityConfigurations()
        {
            try
            {
                var resp = await _service.GetPriorityConfigurations();
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching priority configurations.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("GetPriorityConfigurations/{id}")]
        public async Task<IActionResult> GetPriorityConfigurationById(int id)
        {
            try
            {
                var resp = await _service.GetPriorityConfigurationById(id);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching priority configuration by ID.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPost("CreatePriorityConfiguration")]
        public async Task<IActionResult> CreatePriorityConfiguration(PriorityConfigDto model)
        {
            try
            {
                var resp = await _service.CreatePriorityConfiguration(model);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating priority configuration.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPut("UpdatePriorityConfiguration/{id}")]
        public async Task<IActionResult> UpdatePriorityConfiguration(PriorityConfigDto model)
        {
            try
            {
                var resp = await _service.UpdatePriorityConfiguration(model);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating priority configuration.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpDelete("DeletePriorityConfiguration/{id}")]
        public async Task<IActionResult> DeletePriorityConfiguration(int id)
        {
            try
            {
                var resp = await _service.DeletePriorityConfiguration(id);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting priority configuration.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpGet("CalculatePriority")]
        public async Task<IActionResult> CalculatePriority(string symptoms)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(symptoms))
                {
                    return BadRequest(new { Success = false, Message = "Symptoms cannot be empty", ErrorCode = "EMPTY_SYMPTOMS" });
                }

                var result = await _service.CalculatePriorityAsync(symptoms);

                if (!result.Success)
                {
                    return Ok(new { Success = false, Message = result.Message, Data = result.Data });
                }

                return Ok(new { Success = true, Message = result.Message, Data = result.Data });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { Success = false, Message = $"Argument error: {ex.Message}", ErrorCode = "ARG_NULL_EXCEPTION" });
            }
            catch (FormatException ex)
            {
                return BadRequest(new { Success = false, Message = $"Format error: {ex.Message}", ErrorCode = "FORMAT_EXCEPTION" });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { Success = false, Message = $"Invalid operation: {ex.Message}", ErrorCode = "INVALID_OPERATION" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "An unexpected error occurred while calculating priority", ErrorCode = "INTERNAL_SERVER_ERROR", Details = ex.Message });
            }
        }


        #endregion
        #region TOKEN STATUSES
        [HttpGet("GetTokenStatuses")]
        public async Task<IActionResult> GetTokenStatuses()
        {
            try
            {
                var resp = await _service.GetTokenStatuses();
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching token statuses.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("GetTokenStatuses/{id}")]
        public async Task<IActionResult> GetTokenStatusById(int id)
        {
            try
            {
                return Ok(await _service.GetTokenStatusById(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching token status by ID.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPost("CreateTokenStatus")]
        public async Task<IActionResult> CreateTokenStatus(TokenStatusDTO model)
        {
            try
            {
                return Ok(await _service.CreateTokenStatus(model));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating token status.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPut("UpdateTokenStatus/{id}")]
        public async Task<IActionResult> UpdateTokenStatus(TokenStatusDTO model)
        {
            try
            {
                return Ok(await _service.UpdateTokenStatus(model));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating token status.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpDelete("DeleteTokenStatus/{id}")]
        public async Task<IActionResult> DeleteTokenStatus(int id)
        {
            try
            {
                return Ok(await _service.DeleteTokenStatus(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting token status.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        #endregion
        #region TOKEN GENERATION
        [HttpPost("GenerateToken")]
        public async Task<IActionResult> GenerateToken([FromBody] GenerateTokenRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new { Success = false, Message = "Request body cannot be empty", ErrorCode = "NULL_REQUEST" });
                }
                var result = await _service.GenerateTokenAsync(request);
                if (!result.Success)
                {
                    return Ok(new { Success = false, Message = result.Message, ErrorCode = "TOKEN_GENERATION_FAILED" });
                }
                return Ok(new { Success = true, Message = result.Message, Data = result.Data });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { Success = false, Message = $"Argument error: {ex.Message}", ErrorCode = "ARG_NULL_EXCEPTION" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Success = false, Message = $"Invalid argument: {ex.Message}", ErrorCode = "ARG_EXCEPTION" });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { Success = false, Message = $"Invalid operation: {ex.Message}", ErrorCode = "INVALID_OPERATION" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "An unexpected error occurred while generating token", ErrorCode = "INTERNAL_SERVER_ERROR", Details = ex.Message });
            }
        }
        #endregion
        #endregion


    }
}
