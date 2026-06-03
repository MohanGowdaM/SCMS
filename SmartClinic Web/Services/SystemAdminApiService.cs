using Newtonsoft.Json;
using SmartClinic.Web.Models.SystemAdmin;
using System.Net.Http.Headers;
using System.Text;

namespace SmartClinic.Web.Services
{
    public class SystemAdminApiService
    {
        #region   SystemAdmin ApiService  
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<SystemAdminApiService> _logger;
        public SystemAdminApiService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, ILogger<SystemAdminApiService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClientFactory.CreateClient("SmartClinicAPI");
            _logger = logger;
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
        #region Roles
        public async Task<List<RoleDto>> GetRolesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("SystemAdmin/GetRoles");
                if (!response.IsSuccessStatusCode) return new List<RoleDto>();
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<RoleDto>>(json) ?? new List<RoleDto>();
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                _logger.LogError(ex, "Error fetching roles");
                return new List<RoleDto>();
            }
        }
        public async Task<bool> CreateRoleAsync(RoleDto model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("SystemAdmin/CreateRole", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating role");
                return false;
            }

        }
        public async Task<bool> UpdateRoleAsync(int id, RoleDto model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"SystemAdmin/UpdateRole", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating role");
                return false;
            }

        }
        public async Task<bool> DeleteRoleAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"SystemAdmin/DeleteRole/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting role");
                return false;
            }
        }
        #endregion
        #region Users
        public async Task<List<UserDto>> GetUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("SystemAdmin/users");
                if (!response.IsSuccessStatusCode) return new List<UserDto>();
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<UserDto>>(json) ?? new List<UserDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching users");
                return new List<UserDto>();
            }
        }
        public async Task<bool> CreateUserAsync(UserDto model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("SystemAdmin/CreateUsers", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return false;
            }
        }
        public async Task<bool> UpdateUserAsync(int id, UserDto model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"SystemAdmin/UpdateUser", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user");
                return false;
            }
        }
        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"SystemAdmin/DeleteUser/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user");
                return false;
            }
        }
        #endregion
        #region Departments
        public async Task<List<DepartmentDto>> GetDepartmentsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("SystemAdmin/departments");
                if (!response.IsSuccessStatusCode) return new List<DepartmentDto>();
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<DepartmentDto>>(json) ?? new List<DepartmentDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching departments");
                return new List<DepartmentDto>();
            }
        }
        public async Task<bool> CreateDepartmentAsync(DepartmentDto model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("SystemAdmin/departments", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating department");
                return false;
            }
        }
        public async Task<bool> UpdateDepartmentAsync(int id, DepartmentDto model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"SystemAdmin/departments/{id}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating department");
                return false;
            }

        }
        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"SystemAdmin/departments/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting department");
                return false;
            }
        }
        #endregion
        #region Doctors
        public async Task<List<DoctorDto>> GetDoctorsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("SystemAdmin/GetDoctors");
                if (!response.IsSuccessStatusCode) return new List<DoctorDto>();
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<DoctorDto>>(json) ?? new List<DoctorDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching doctors");
                return new List<DoctorDto>();
            }
        }
        public async Task<bool> CreateDoctorAsync(DoctorDto model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("SystemAdmin/CreateDoctor", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating doctor");
                return false;
            }
        }
        public async Task<bool> UpdateDoctorAsync(int id, DoctorDto model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"SystemAdmin/UpdateDoctor", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating doctor");
                return false;
            }
        }
        public async Task<bool> DeleteDoctorAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"SystemAdmin/DeleteDoctor?id={id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting doctor");
                return false;
            }
        }
        #endregion
        #region Symptoms
        public async Task<List<SymptomDto>> GetSymptomsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("SystemAdmin/GetSymptoms");
                if (!response.IsSuccessStatusCode) return new List<SymptomDto>();
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<SymptomDto>>(json) ?? new List<SymptomDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching symptoms");
                return new List<SymptomDto>();
            }
        }
        public async Task<bool> CreateSymptomAsync(SymptomDto model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("SystemAdmin/CreateSymptom", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating symptom");
                return false;
            }
        }
        public async Task<bool> UpdateSymptomAsync(int id, SymptomDto model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"SystemAdmin/UpdateSymptom", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating symptom");
                return false;
            }

        }
        public async Task<bool> DeleteSymptomAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"SystemAdmin/DeleteSymptom/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting symptom");
                return false;
            }
        }
        #endregion
        #region Priority Configs
        public async Task<List<PriorityConfigDto>> GetPriorityConfigsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("SystemAdmin/GetPriorityConfigurations");
                if (!response.IsSuccessStatusCode) return new List<PriorityConfigDto>();
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<PriorityConfigDto>>(json) ?? new List<PriorityConfigDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching priority configs");
                return new List<PriorityConfigDto>();
            }
        }
        public async Task<bool> CreatePriorityConfigAsync(PriorityConfigDto model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("SystemAdmin/CreatePriorityConfiguration", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating priority config");
                return false;
            }
        }
        public async Task<bool> UpdatePriorityConfigAsync(int id, PriorityConfigDto model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"SystemAdmin/UpdatePriorityConfiguration/{id}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating priority config");
                return false;
            }
        }
        public async Task<bool> DeletePriorityConfigAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"SystemAdmin/DeletePriorityConfiguration/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting priority config");
                return false;
            }
        }
        #endregion
        #region Token Statuses
        public async Task<List<TokenStatusDto>> GetTokenStatusesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("SystemAdmin/GetTokenStatuses");
                if (!response.IsSuccessStatusCode) return new List<TokenStatusDto>();
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<TokenStatusDto>>(json) ?? new List<TokenStatusDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching token statuses");
                return new List<TokenStatusDto>();
            }
        }
        public async Task<bool> CreateTokenStatusAsync(TokenStatusDto model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("SystemAdmin/CreateTokenStatus", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating token status");
                return false;
            }
        }
        public async Task<bool> UpdateTokenStatusAsync(int id, TokenStatusDto model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"SystemAdmin/UpdateTokenStatus/{id}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating token status");
                return false;
            }
        }
        public async Task<bool> DeleteTokenStatusAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"SystemAdmin/DeleteTokenStatus/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting token status");
                return false;
            }
        }
        #endregion
        #endregion

    }
}
