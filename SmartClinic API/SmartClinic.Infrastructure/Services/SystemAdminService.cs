using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartClinic.Application.DTOs;
using SmartClinic.Application.Interfaces;
using SmartClinic.Domain.Entities;
using SmartClinic.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
namespace SmartClinic.Infrastructure.Services
{
    public class SystemAdminService : ISystemAdminService
    {
        #region System Admin
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ITokenSequenceService _tokenSequenceService;
        private readonly ILogger<SystemAdminService> _logger;

        public SystemAdminService(ApplicationDbContext context, IEmailService emailService, ITokenSequenceService tokenSequenceService, ILogger<SystemAdminService> logger)
        {
            _context = context;
            _emailService = emailService;
            _tokenSequenceService = tokenSequenceService;
            _logger = logger;
        }
        #region ROLES
        public async Task<List<RolesDto>> GetRoles()
        {
            try
            {
                var roles = new List<RolesDto>();
                var data = await _context.Roles.Where(x => x.IsDeleted == false).ToListAsync();
                foreach (var role in data)
                {
                    roles.Add(new RolesDto
                    {
                        Id = role.Id,
                        Name = role.Name,
                        Discription = role.Discription,
                        IsDeleted = role.IsDeleted
                    });
                }
                return roles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching roles");
                return new List<RolesDto>();
            }
        }
        public async Task<RolesDto?> GetRoleById(int id)
        {
            try
            {
                var data = await _context.Roles.Select(x => new RolesDto
                {
                    Name = x.Name,
                    IsDeleted = x.IsDeleted,
                }).FirstOrDefaultAsync(x => x.Id == id);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching role with ID {id}");
                return null;
            }
        }
        public async Task<ApiResponse<RolesDto>> CreateRole(RolesDto model)
        {
            try
            {
                var role = new Role
                {
                    Name = model.Name,
                    IsDeleted = false,
                    Discription = model.Discription
                };
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();
                return new ApiResponse<RolesDto> { Success = true, Data = model };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating role");
                return new ApiResponse<RolesDto> { Success = false, Data = model };
            }
        }
        public async Task<ApiResponse<RolesDto>> UpdateRole(RolesDto model)
        {
            try
            {
                var data = await _context.Roles.FirstOrDefaultAsync(x => x.Id == model.Id);
                if (data == null)
                {
                    return new ApiResponse<RolesDto> { Success = false, Message = "Role not found" };
                }
                data.Name = model.Name;
                data.IsDeleted = data.IsDeleted;
                data.Discription = model.Discription;
                await _context.SaveChangesAsync();
                return new ApiResponse<RolesDto> { Success = true, Data = model };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating role with ID {model.Id}");
                return new ApiResponse<RolesDto> { Success = false, Data = model };
            }
        }
        public async Task<ApiResponse<bool>> DeleteRole(int id)
        {
            try
            {
                var data = await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);
                if (data == null)
                {
                    return new ApiResponse<bool> { Success = false, Message = "Role not found" };
                }
                else
                {
                    data.IsDeleted = true;
                    _context.Roles.Update(data);
                    await _context.SaveChangesAsync();
                    return new ApiResponse<bool> { Success = true, Data = true };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting role with ID {id}");
                return new ApiResponse<bool> { Success = false, Data = false };
            }
        }
        #endregion
        #region DEPARTMENTS
        public async Task<List<DepartmentDto>> GetDepartments()
        {
            try
            {
                var departments = await _context.Departments.Where(x => x.IsDeleted == false).ToListAsync();
                return departments.Select(x => new DepartmentDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    IsDeleted = x.IsDeleted
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching departments");
                return new List<DepartmentDto>();
            }
        }
        public async Task<ApiResponse<DepartmentDto>> CreateDepartment(DepartmentDto model)
        {
            try
            {
                var department = new Department
                {
                    Name = model.Name,
                    IsDeleted = false
                };
                await _context.Departments.AddAsync(department);
                await _context.SaveChangesAsync();
                return new ApiResponse<DepartmentDto> { Success = true, Data = model };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating department");
                return new ApiResponse<DepartmentDto> { Success = false, Data = model };
            }
        }
        public async Task<DepartmentDto?> GetDepartmentById(int id)
        {
            try
            {
                var data = await _context.Departments.Select(x => new DepartmentDto
                {
                    Name = x.Name,
                    IsDeleted = x.IsDeleted
                }).FirstOrDefaultAsync(x => x.Id == id);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching department with ID {id}");
                return null;
            }
        }
        public async Task<ApiResponse<DepartmentDto>> UpdateDepartment(DepartmentDto model)
        {
            try
            {
                var data = await _context.Departments.FirstOrDefaultAsync(x => x.Id == model.Id);

                if (data == null)
                {
                    return new ApiResponse<DepartmentDto> { Success = false, Message = "Department not found" };
                }
                data.Name = model.Name;
                data.Description = model.Description;
                data.IsDeleted = data.IsDeleted;
                data.UpdatedDateTime = DateTime.Now;
                await _context.SaveChangesAsync();
                return new ApiResponse<DepartmentDto> { Success = true, Data = model };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating department with ID {model.Id}");
                return new ApiResponse<DepartmentDto> { Success = false, Data = model };
            }
        }
        public async Task<ApiResponse<bool>> DeleteDepartment(int id)
        {
            try
            {
                var data = await _context.Departments.FirstOrDefaultAsync(x => x.Id == id);
                if (data == null)
                {
                    return new ApiResponse<bool> { Success = false, Message = "Department not found" };
                }
                data.IsDeleted = true;
                _context.Departments.Update(data);
                await _context.SaveChangesAsync();
                return new ApiResponse<bool> { Success = true, Data = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting department with ID {id}");
                return new ApiResponse<bool> { Success = false, Data = false };
            }
        }
        #endregion
        #region DOCTORS
        public async Task<List<DoctorDto>> GetDoctors()
        {
            try
            {
                return await _context.Doctors
                    .Include(x => x.Department)
                    .Select(x => new DoctorDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        DepartmentId = x.DepartmentId,
                        ContactNumber = x.ContactNumber,
                        DepartmentName = x.Department != null ? x.Department.Name : null,
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching doctors");
                return new List<DoctorDto>();
            }
        }
        public async Task<ApiResponse<DoctorDto>> CreateDoctor(DoctorDto model)
        {
            try
            {
                var doctor = new Doctor
                {
                    Name = model.Name,
                    DepartmentId = model.DepartmentId,
                    IsAvailable = model.IsAvailable,
                    ContactNumber = model.ContactNumber
                };
                _context.Doctors.Add(doctor);
                await _context.SaveChangesAsync();
                return new ApiResponse<DoctorDto> { Success = true, Data = model };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating doctor");
                return new ApiResponse<DoctorDto> { Success = false, Data = model };
            }
        }
        public async Task<DoctorDto?> GetDoctorById(int id)
        {
            try
            {
                var data = await _context.Doctors.Select(x => new DoctorDto
                {
                    Name = x.Name,
                    IsAvailable = x.IsAvailable,
                    ContactNumber = x.ContactNumber,
                    DepartmentId = x.DepartmentId,
                    DepartmentName = x.Department != null ? x.Department.Name : null,
                }).FirstOrDefaultAsync(x => x.Id == id);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching doctor with ID {id}");
                return null;
            }
        }
        public async Task<ApiResponse<DoctorDto>> UpdateDoctor(DoctorDto model)
        {
            try
            {
                var data = await _context.Doctors.FirstOrDefaultAsync(x => x.Id == model.Id);
                if (data == null)
                {
                    return new ApiResponse<DoctorDto> { Success = false, Message = "Doctor not found" };
                }
                data.Name = model.Name;
                data.ContactNumber = model.ContactNumber;
                data.DepartmentId = model.DepartmentId;
                data.IsAvailable = model.IsAvailable;
                _context.Doctors.Update(data);
                await _context.SaveChangesAsync();
                return new ApiResponse<DoctorDto> { Success = true, Data = model };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating doctor with ID {model.Id}");
                return new ApiResponse<DoctorDto> { Success = false, Data = model };
            }
        }
        public async Task<ApiResponse<bool>> DeleteDoctor(int id)
        {
            try
            {
                var data = await _context.Doctors.FirstOrDefaultAsync(x => x.Id == id);
                if (data == null)
                {
                    return new ApiResponse<bool> { Success = false, Message = "Doctor not found" };
                }
                data.IsAvailable = false;
                _context.Doctors.Update(data);
                await _context.SaveChangesAsync();
                return new ApiResponse<bool> { Success = true, Data = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting doctor with ID {id}");
                return new ApiResponse<bool> { Success = false, Data = false };
            }
        }
        public async Task<List<DoctorDto>> GetDoctorsByDepartmentAsync(int departmentId)
        {
            try
            {
                if (departmentId <= 0)
                {
                    _logger.LogWarning($"Invalid department ID: {departmentId}. Department ID must be greater than zero.");
                    return new List<DoctorDto>();
                }
                // Verify department exists
                var departmentExists = await _context.Departments.AnyAsync(d => d.Id == departmentId && d.IsDeleted == false);
                if (!departmentExists)
                {
                    _logger.LogWarning($"Department with ID {departmentId} not found or has been deleted");
                    return new List<DoctorDto>();
                }
                // Get doctors by department
                var doctors = await _context.Doctors
                    .Where(d => d.DepartmentId == departmentId && d.IsAvailable == true)
                    .Include(d => d.Department)
                    .Select(d => new DoctorDto
                    {
                        Id = d.Id,
                        Name = d.Name,
                        DepartmentId = d.DepartmentId,
                        DepartmentName = d.Department != null ? d.Department.Name : null,
                        ContactNumber = d.ContactNumber,
                        IsAvailable = d.IsAvailable,
                        ConsultationTimeInMin = d.ConsultationTimeInMin,
                        CreatedDateTime = d.CreatedDateTime,
                        CreatedBy = d.CreatedBy,
                        UpdatedDateTime = d.UpdatedDateTime,
                        UpdatedBy = d.UpdatedBy,
                        DepartmentDto = d.Department != null ? new DepartmentDto
                        {
                            Id = d.Department.Id,
                            Name = d.Department.Name,
                            Description = d.Department.Description,
                            IsDeleted = d.Department.IsDeleted
                        } : null
                    })
                    .ToListAsync();
                if (!doctors.Any())
                {
                    _logger.LogWarning($"No available doctors found for department ID: {departmentId}");
                }

                return doctors;
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, $"Argument null exception while fetching doctors for department {departmentId}: {ex.Message}");
                return new List<DoctorDto>();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"Database update exception while fetching doctors for department {departmentId}: {ex.Message}");
                return new List<DoctorDto>();
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, $"Operation cancelled while fetching doctors for department {departmentId}: {ex.Message}");
                return new List<DoctorDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error occurred while fetching doctors for department {departmentId}: {ex.Message}");
                return new List<DoctorDto>();
            }
        }
        #endregion
        #region SYMPTOMS
        public async Task<List<SymptomsDto>> GetSymptoms()
        {
            try
            {
                return await _context.Symptoms.Where(x => x.IsDeleted == false).Select(x => new SymptomsDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Weight = x.Weight
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching symptoms.");
                return new List<SymptomsDto>();
            }
        }
        public async Task<ApiResponse<SymptomsDto>> CreateSymptom(SymptomsDto model)
        {
            try
            {
                var symptom = new Symptom
                {
                    Name = model.Name,
                    Weight = model.Weight,
                    IsDeleted = false
                };
                _context.Symptoms.Add(symptom);
                await _context.SaveChangesAsync();
                return new ApiResponse<SymptomsDto> { Success = true, Data = model };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating symptom.");
                return new ApiResponse<SymptomsDto> { Success = false, Data = model };
            }
        }
        public async Task<SymptomsDto?> GetSymptomById(int id)
        {
            try
            {
                var data = await _context.Symptoms.Select(n => new SymptomsDto
                {
                    Id = n.Id,
                    Name = n.Name
                }).FirstOrDefaultAsync(x => x.Id == id);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching symptom with ID {id}");
                return null;
            }
        }
        public async Task<ApiResponse<SymptomsDto>> UpdateSymptom(SymptomsDto model)
        {
            try
            {
                var data = await _context.Symptoms.FirstOrDefaultAsync(x => x.Id == model.Id);
                if (data == null)
                {
                    return new ApiResponse<SymptomsDto> { Success = false, Message = "Symptom not found" };
                }
                data.Name = model.Name;
                data.Weight = model.Weight;
                data.IsDeleted = data.IsDeleted;
                _context.Symptoms.Update(data);
                await _context.SaveChangesAsync();
                return new ApiResponse<SymptomsDto> { Success = true, Data = model };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating symptom with ID {model.Id}");
                return new ApiResponse<SymptomsDto> { Success = false, Data = model };
            }
        }
        public async Task<ApiResponse<bool>> DeleteSymptom(int id)
        {
            try
            {
                var data = await _context.Symptoms.FirstOrDefaultAsync(x => x.Id == id);
                if (data == null)
                {
                    return new ApiResponse<bool> { Success = false, Message = "Symptom not found" };
                }
                data.IsDeleted = true;
                _context.Symptoms.Update(data);
                await _context.SaveChangesAsync();
                return new ApiResponse<bool> { Success = true, Data = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting symptom with ID {id}");
                return new ApiResponse<bool> { Success = false, Data = false };
            }
        }
        #endregion
        #region Users
        public async Task<List<UserDto>> GetUsers()
        {
            try
            {
                var users = await _context.Users
                    .Include(u => u.Role).Include(d => d.Doctors)
                    .Where(u => u.IsDeleted == false)
                    .Select(u => new UserDto
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Email = u.Email,
                        Username = u.Username,
                        PasswordHash = u.PasswordHash,
                        RoleId = u.RoleId,
                        IsDeleted = u.IsDeleted,
                        CreatedDateTime = u.CreatedDateTime,
                        CreatedBy = u.CreatedBy,
                        UpdatedDateTime = u.UpdatedDateTime,
                        UpdatedBy = u.UpdatedBy,
                        RoleName = u.Role.Name,
                        ContactNumber = u.Doctors.Where(d => d.UserId == u.Id).Select(d => d.ContactNumber).FirstOrDefault(),
                        RolesDto = new RolesDto
                        {
                            Id = u.Role.Id,
                            Name = u.Role.Name,
                            IsDeleted = u.Role.IsDeleted
                        }
                    })
                    .ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching users: {ex.Message}");
                return await Task.FromResult(new List<UserDto>());
            }
        }
        public async Task<UserDto?> GetUserById(int id)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Role) // Include the related Roles entity
                    .Where(u => u.Id == id && u.IsDeleted == false) // Filter by ID and exclude deleted users
                    .Select(u => new UserDto
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Username = u.Username,
                        RoleId = u.RoleId,
                        IsDeleted = u.IsDeleted,
                        CreatedDateTime = u.CreatedDateTime,
                        CreatedBy = u.CreatedBy,
                        UpdatedDateTime = u.UpdatedDateTime,
                        UpdatedBy = u.UpdatedBy,
                        RolesDto = new RolesDto
                        {
                            Id = u.Role.Id,
                            Name = u.Role.Name,
                            IsDeleted = u.Role.IsDeleted
                        }
                    })
                    .FirstOrDefaultAsync();
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching user: {ex.Message}");
                return await Task.FromResult<UserDto?>(null);
            }
        }
        public async Task<ApiResponse<UserDto>> CreateUser(UserDto model)
        {
            try
            {
                model.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash);
                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Username = model.Username,
                    PasswordHash = model.PasswordHash,
                    RoleId = model.RoleId,
                    IsDeleted = false,
                    CreatedDateTime = DateTime.Now,
                    CreatedBy = model.CreatedBy // You can replace this with the actual user creating the record
                };
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                if (model.RoleId == 2)
                {
                    var doctor = new Doctor
                    {
                        Name = model.Username,
                        UserId = user.Id,
                        DepartmentId = model.DepartmentId, // You can set this based on your requirements
                        IsAvailable = true,
                        ContactNumber = model.ContactNumber // You can set this based on your requirements
                    };
                    await _context.Doctors.AddAsync(doctor);
                    await _context.SaveChangesAsync();
                }
                return new ApiResponse<UserDto> { Success = true, Data = model };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while creating user: {ex.Message}");
                return new ApiResponse<UserDto> { Success = false, Data = model };
            }
        }
        public async Task<ApiResponse<UserDto>> UpdateUser(UserDto model)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.Id);
                if (user == null)
                {
                    return new ApiResponse<UserDto> { Success = false, Message = "User not found", Data = model };
                }
                else
                {
                    if (user.PasswordHash == model.PasswordHash)
                    {
                        user.Name = model.Username;
                        user.Email = model.Email;
                        user.Username = model.Username;
                        user.RoleId = model.RoleId;
                        user.IsDeleted = user.IsDeleted;
                        user.UpdatedDateTime = DateTime.UtcNow;
                        user.UpdatedBy = "SystemAdmin"; // You can replace this with the actual user updating the record
                        _context.Users.Update(user);
                        await _context.SaveChangesAsync();
                        if (model.RoleId == 2)
                        {
                            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == user.Id);
                            if (doctor != null)
                            {
                                doctor.Name = model.Username;
                                doctor.DepartmentId = model.DepartmentId; // You can set this based on your requirements
                                doctor.IsAvailable = true;
                                doctor.ContactNumber = model.ContactNumber; // You can set this based on your requirements
                                _context.Doctors.Update(doctor);
                                await _context.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == user.Id);
                            if (doctor != null)
                            {
                                doctor.IsAvailable = false;
                                _context.Doctors.Update(doctor);
                                await _context.SaveChangesAsync();
                            }
                        }
                        return new ApiResponse<UserDto> { Success = true, Data = model };
                    }
                    else
                    {

                        model.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash);
                        user.Name = model.Username;
                        user.Email = model.Email;
                        user.Username = model.Username;
                        user.PasswordHash = model.PasswordHash;
                        user.RoleId = model.RoleId;
                        user.IsDeleted = user.IsDeleted;
                        user.UpdatedDateTime = DateTime.UtcNow;
                        user.UpdatedBy = "SystemAdmin"; // You can replace this with the actual user updating the record
                        _context.Users.Update(user);
                        await _context.SaveChangesAsync();
                        if (model.RoleId == 2)
                        {
                            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == user.Id);
                            if (doctor != null)
                            {
                                doctor.Name = model.Username;
                                doctor.DepartmentId = model.DepartmentId; // You can set this based on your requirements
                                doctor.IsAvailable = true;
                                doctor.ContactNumber = model.ContactNumber; // You can set this based on your requirements
                                _context.Doctors.Update(doctor);
                                await _context.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == user.Id);
                            if (doctor != null)
                            {
                                doctor.IsAvailable = false;
                                _context.Doctors.Update(doctor);
                                await _context.SaveChangesAsync();
                            }
                        }
                        return new ApiResponse<UserDto> { Success = true, Data = model };
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating user: {ex.Message}");
                return new ApiResponse<UserDto> { Success = false, Data = model };
            }
        }
        public async Task<ApiResponse<bool>> DeleteUser(int id)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    return new ApiResponse<bool> { Success = false, Message = "User not found", Data = false };
                }
                else
                {
                    user.IsDeleted = true;
                    user.UpdatedDateTime = DateTime.UtcNow;
                    user.UpdatedBy = "SystemAdmin"; // You can replace this with the actual user deleting the record
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    if (user.RoleId == 2)
                    {
                        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == user.Id);
                        if (doctor != null)
                        {
                            doctor.IsAvailable = false;
                            _context.Doctors.Update(doctor);
                            await _context.SaveChangesAsync();
                        }
                    }
                    return new ApiResponse<bool> { Success = true, Data = true };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting user: {ex.Message}");
                return new ApiResponse<bool> { Success = false, Data = false };
            }
        }
        #endregion
        #region Priority Configuration
        public async Task<List<PriorityConfigDto>> GetPriorityConfigurations()
        {
            try
            {
                var data = await _context.PriorityConfigs.Where(x => x.IsDeleted == false)
                     .Select(x => new PriorityConfigDto
                     {
                         Id = x.Id,
                         Name = x.Name,
                         PriorityLevel = x.PriorityLevel,
                         MinWeight = x.MinWeight,
                         MaxWeight = x.MaxWeight,
                         IsDeleted = x.IsDeleted
                     })
                     .ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching priority configurations: {ex.Message}");
                return await Task.FromResult(new List<PriorityConfigDto>());
            }
        }
        public async Task<PriorityConfigDto?> GetPriorityConfigurationById(int id)
        {
            try
            {
                var data = await _context.PriorityConfigs
                    .Where(x => x.Id == id)
                    .Select(x => new PriorityConfigDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        PriorityLevel = x.PriorityLevel,
                        MinWeight = x.MinWeight,
                        MaxWeight = x.MaxWeight,
                        IsDeleted = x.IsDeleted
                    })
                    .FirstOrDefaultAsync();
                return data;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching priority configuration: {ex.Message}");
                return await Task.FromResult<PriorityConfigDto?>(null);
            }
        }
        public async Task<ApiResponse<PriorityConfigDto>> CreatePriorityConfiguration(PriorityConfigDto model)
        {
            try
            {
                var priorityConfig = new PriorityConfig
                {
                    Name = model.Name,
                    PriorityLevel = model.PriorityLevel,
                    MinWeight = model.MinWeight,
                    MaxWeight = model.MaxWeight,
                    IsDeleted = false
                };
                await _context.PriorityConfigs.AddAsync(priorityConfig);
                await _context.SaveChangesAsync();
                return new ApiResponse<PriorityConfigDto> { Success = true, Data = model };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while creating priority configuration: {ex.Message}");
                return new ApiResponse<PriorityConfigDto> { Success = false, Data = model };
            }
        }
        public async Task<ApiResponse<PriorityConfigDto>> UpdatePriorityConfiguration(PriorityConfigDto model)
        {
            try
            {
                var priorityConfig = await _context.PriorityConfigs.FindAsync(model.Id);
                if (priorityConfig == null)
                {
                    return new ApiResponse<PriorityConfigDto> { Success = false, Data = model };
                }

                priorityConfig.Name = model.Name;
                priorityConfig.PriorityLevel = model.PriorityLevel;
                priorityConfig.MinWeight = model.MinWeight;
                priorityConfig.MaxWeight = model.MaxWeight;
                await _context.SaveChangesAsync();
                return new ApiResponse<PriorityConfigDto> { Success = true, Data = model };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating priority configuration: {ex.Message}");
                return new ApiResponse<PriorityConfigDto> { Success = false, Data = model };
            }
        }
        public async Task<ApiResponse<bool>> DeletePriorityConfiguration(int id)
        {
            try
            {
                var priorityConfig = await _context.PriorityConfigs.FindAsync(id);
                if (priorityConfig == null)
                {
                    return new ApiResponse<bool> { Success = false, Data = false };
                }

                priorityConfig.IsDeleted = true;
                await _context.SaveChangesAsync();
                return new ApiResponse<bool> { Success = true, Data = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting priority configuration: {ex.Message}");
                return new ApiResponse<bool> { Success = false, Data = false };
            }
        }
        #endregion
        #region TokenStatus
        public async Task<List<TokenStatusDTO>> GetTokenStatuses()
        {
            try
            {
                var tokenStatuses = await _context.TokenStatuses.Where(x => x.IsDeleted == false)
                    .Select(x => new TokenStatusDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                    })
                    .ToListAsync();
                return tokenStatuses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching token statuses: {ex.Message}");
                return await Task.FromResult(new List<TokenStatusDTO>());
            }
        }
        public async Task<ApiResponse<TokenStatusDTO>> CreateTokenStatus(TokenStatusDTO model)
        {
            try
            {
                var tokenStatus = new TokenStatus
                {
                    Name = model.Name,
                    IsDeleted = false
                };
                await _context.TokenStatuses.AddAsync(tokenStatus);
                await _context.SaveChangesAsync();
                return new ApiResponse<TokenStatusDTO> { Success = true, Data = model };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while creating token status: {ex.Message}");
                return new ApiResponse<TokenStatusDTO> { Success = false, Data = model };
            }
        }
        public async Task<TokenStatusDTO?> GetTokenStatusById(int id)
        {
            try
            {
                var tokenStatus = await _context.TokenStatuses
                    .Where(x => x.Id == id)
                    .Select(x => new TokenStatusDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                    })
                    .FirstOrDefaultAsync();
                return tokenStatus;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching token status: {ex.Message}");
                return new TokenStatusDTO();
            }
        }
        public async Task<ApiResponse<TokenStatusDTO>> UpdateTokenStatus(TokenStatusDTO model)
        {
            try
            {
                var tokenStatus = await _context.TokenStatuses.FirstOrDefaultAsync(x => x.Id == model.Id);
                if (tokenStatus == null)
                {
                    return new ApiResponse<TokenStatusDTO> { Success = false, Message = "Token status not found", Data = model };
                }
                else
                {
                    tokenStatus.Name = model.Name;
                    _context.TokenStatuses.Update(tokenStatus);
                    await _context.SaveChangesAsync();
                    return new ApiResponse<TokenStatusDTO> { Success = true, Data = model };
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating token status: {ex.Message}");
                return new ApiResponse<TokenStatusDTO> { Success = false, Data = model };
            }
        }
        public async Task<ApiResponse<bool>> DeleteTokenStatus(int id)
        {
            try
            {
                var tokenStatus = await _context.TokenStatuses.FirstOrDefaultAsync(x => x.Id == id);
                if (tokenStatus == null)
                {
                    return new ApiResponse<bool> { Success = false, Message = "Token status not found", Data = false };
                }
                else
                {
                    tokenStatus.IsDeleted = true;
                    _context.TokenStatuses.Update(tokenStatus);
                    await _context.SaveChangesAsync();
                    return new ApiResponse<bool> { Success = true, Data = true };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting token status: {ex.Message}");
                return new ApiResponse<bool> { Success = false, Data = false };
            }
        }
        #endregion
        #region Priority Calculation
        public async Task<ApiResponse<PriorityCalculationResultDto>> CalculatePriorityAsync(string symptoms)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(symptoms))
                {
                    return new ApiResponse<PriorityCalculationResultDto> { Success = false, Message = "Symptoms cannot be empty", Data = null };
                }
                // Parse symptoms
                var symptomList = symptoms
                    .Split(',')
                    .Select(x => x.Trim())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct()
                    .ToList();
                if (!symptomList.Any())
                {
                    return new ApiResponse<PriorityCalculationResultDto> { Success = false, Message = "No valid symptoms found", Data = null };
                }
                // Get matching symptoms from DB
                var matchedSymptoms = await _context.Symptoms.Where(s => symptomList.Contains(s.Name) && s.IsDeleted == false).ToListAsync();
                if (!matchedSymptoms.Any())
                {
                    return new ApiResponse<PriorityCalculationResultDto> { Success = false, Message = "No matching symptoms found", Data = null };
                }
                // Calculate total symptom weight
                var totalWeight = matchedSymptoms.Sum(x => x.Weight ?? 0);
                _logger.LogInformation($"Total Symptom Weight: {totalWeight}");
                // Find matching priority
                var priority =
                    await _context.PriorityConfigs
                    .Where(p => p.IsDeleted == false && totalWeight >= (p.MinWeight ?? 0) && totalWeight <= (p.MaxWeight ?? 0))
                    .OrderByDescending(p => p.PriorityLevel)
                    .FirstOrDefaultAsync();

                if (priority == null)
                {
                    return new ApiResponse<PriorityCalculationResultDto> { Success = false, Message = $"No priority found for weight: {totalWeight}", Data = null };
                }

                // Map result
                var result = new PriorityCalculationResultDto
                {
                    Id = priority.Id,
                    Name = priority.Name,
                    PriorityLevel = priority.PriorityLevel,
                    MinWeight = priority.MinWeight,
                    MaxWeight = priority.MaxWeight,
                    TotalSymptomWeight = totalWeight,
                    MatchedSymptoms = matchedSymptoms.Select(x => x.Name).ToList()
                };
                return new ApiResponse<PriorityCalculationResultDto> { Success = true, Message = $"Priority calculated successfully using {matchedSymptoms.Count} symptom(s)", Data = result };
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Priority calculation error: {ex.Message}");
                return new ApiResponse<PriorityCalculationResultDto> { Success = false, Message = "An unexpected error occurred while calculating priority", Data = null };
            }
        }
        #endregion
        #region Token Generation
        public async Task<ApiResponse<TokenDto>> GenerateTokenAsync(GenerateTokenRequest request)
        {
            try
            {
                // Validate request
                if (request == null)
                {
                    return new ApiResponse<TokenDto> { Success = false, Message = "Request cannot be null" };
                }
                if (request.PatientId <= 0)
                {
                    return new ApiResponse<TokenDto> { Success = false, Message = "Valid PatientId is required" };
                }
                if (request.DepartmentId <= 0)
                {
                    return new ApiResponse<TokenDto> { Success = false, Message = "Valid DepartmentId is required" };
                }
                // Verify patient exists
                var patient = await _context.Patients.FirstOrDefaultAsync(x => x.Id == request.PatientId && x.IsDeleted == false);
                if (patient == null)
                {
                    return new ApiResponse<TokenDto> { Success = false, Message = "Patient not found" };
                }
                // Verify department exists
                var department = await _context.Departments.FirstOrDefaultAsync(x => x.Id == request.DepartmentId && x.IsDeleted == false);
                if (department == null)
                {
                    return new ApiResponse<TokenDto> { Success = false, Message = "Department not found" };
                }
                // Verify doctor
                Doctor doctor = null;
                if (request.DoctorId != null && request.DoctorId > 0)
                {
                    doctor = await _context.Doctors.FirstOrDefaultAsync(x => x.Id == request.DoctorId && x.IsAvailable == true);
                    if (doctor == null)
                    {
                        return new ApiResponse<TokenDto>
                        {
                            Success = false,
                            Message = "Doctor not found or unavailable"
                        };
                    }
                }
                // Get pending token status
                var pendingStatus = await _context.TokenStatuses.FirstOrDefaultAsync(x => x.Id == 1 && x.IsDeleted == false);
                if (pendingStatus == null)
                {
                    return new ApiResponse<TokenDto> { Success = false, Message = "Default token status not found" };
                }

                var serviceID = await _context.Services.FirstOrDefaultAsync(x => x.Id == 2 && x.IsDeleted == false);
                if (serviceID == null)
                {
                    return new ApiResponse<TokenDto> { Success = false, Message = "Default service not found" };
                }

                // Get priority name
                var priorityName = await _context.PriorityConfigs.Where(x => x.Id == request.PriorityId).Select(x => x.Name).FirstOrDefaultAsync();
                if (request.IsReschedule && request.ExistingTokenId.HasValue)
                {
                    var existingToken = await _context.Tokens.Include(x => x.TokenSymptoms).FirstOrDefaultAsync(x => x.Id == request.ExistingTokenId.Value && x.IsDeleted == false);
                    if (existingToken != null)
                    {
                        // Update token
                        existingToken.AppointmentDateTime = request.AppointmentDateTime;
                        existingToken.DepartmentId = request.DepartmentId;
                        existingToken.DoctorId = request.DoctorId;
                        existingToken.Priority = request.PriorityId;
                        // Remove old symptoms
                        _context.TokenSymptoms.RemoveRange(existingToken.TokenSymptoms);
                        // Add new symptoms
                        if (request.Symptoms != null && request.Symptoms.Any())
                        {
                            var symptomIds = request.Symptoms.Select(int.Parse).ToList();
                            foreach (var symptomId in symptomIds)
                            {
                                await _context.TokenSymptoms.AddAsync(new TokenSymptom
                                {
                                    TokenId = existingToken.Id,
                                    SymptomId = symptomId
                                });
                            }
                            // Save symptom names
                            var symptoms = await _context.Symptoms.Where(x => symptomIds.Contains(x.Id) && x.IsDeleted == false).ToListAsync();
                            string symptomName = string.Join(", ", symptoms.Select(x => x.Name));
                            patient.Symptoms = symptomName;
                        }
                        await _context.SaveChangesAsync();
                        return new ApiResponse<TokenDto> { Success = true, Message = "Appointment rescheduled successfully", Data = new TokenDto { Id = existingToken.Id, TokenNumber = existingToken.TokenNumber } };
                    }
                }
                // Generate token number based on department
                var tokenNumber = await _tokenSequenceService.GenerateTokenNumberAsync(request.DepartmentId, department.Name);
                // Create token
                var token = new Token
                {
                    TokenNumber = tokenNumber,
                    PatientId = request.PatientId,
                    DepartmentId = request.DepartmentId,
                    DoctorId = request.DoctorId,
                    Priority = request.PriorityId,
                    StatusId = pendingStatus.Id,
                    ServiceId = serviceID.Id,
                    CreatedAt = DateTime.Now,
                    AppointmentDateTime = request.AppointmentDateTime,
                    IsDeleted = false
                };
                await _context.Tokens.AddAsync(token);
                await _context.SaveChangesAsync();
                // ====================================
                // Save Symptoms
                // ====================================
                string symptomNames = string.Empty;
                // Selected symptoms
                if (request.Symptoms != null && request.Symptoms.Any())
                {
                    var symptomIds = request.Symptoms.Where(x => int.TryParse(x, out _)).Select(int.Parse).ToList();
                    if (symptomIds.Any())
                    {
                        var symptoms = await _context.Symptoms.Where(x => symptomIds.Contains(x.Id) && x.IsDeleted == false).ToListAsync();
                        // Save symptom names
                        symptomNames = string.Join(", ", symptoms.Select(x => x.Name));
                        patient.Symptoms = symptomNames;
                        // Save token symptoms
                        foreach (var symptom in symptoms)
                        {
                            await _context.TokenSymptoms.AddAsync(new TokenSymptom { TokenId = token.Id, SymptomId = symptom.Id });
                        }
                    }
                }
                // Custom symptom
                if (!string.IsNullOrWhiteSpace(request.OtherSymptoms))
                {
                    if (!string.IsNullOrWhiteSpace(patient.Symptoms))
                    {
                        patient.Symptoms += ", " + request.OtherSymptoms;
                    }
                    else
                    {
                        patient.Symptoms = request.OtherSymptoms;
                    }
                    patient.Symptoms = request.OtherSymptoms;
                }
                // Update patient
                _context.Patients.Update(patient);
                await _context.SaveChangesAsync();
                // Fetch created token
                var createdToken = await _context.Tokens
                        .Include(x => x.Patient)
                        .Include(x => x.Department)
                        .Include(x => x.Doctor)
                        .Include(x => x.Status)
                        .Include(x => x.TokenSymptoms)
                            .ThenInclude(x => x.Symptom)
                        .FirstOrDefaultAsync(x => x.Id == token.Id);
                // Map DTO
                var tokenDto = new TokenDto
                {
                    Id = createdToken.Id,
                    TokenNumber = createdToken.TokenNumber,
                    PatientId = createdToken.PatientId,
                    DepartmentId = createdToken.DepartmentId,
                    DoctorId = createdToken.DoctorId,
                    Priority = createdToken.Priority,
                    StatusId = createdToken.StatusId,
                    CreatedAt = createdToken.CreatedAt,
                    CalledAt = createdToken.CalledAt,
                    CompletedAt = createdToken.CompletedAt,
                    PatientDto = createdToken.Patient != null
                        ? new PatientDto
                        {
                            Id = createdToken.Patient.Id,
                            Name = createdToken.Patient.Name,
                            Uhid = createdToken.Patient.Uhid,
                            MobileNumber = createdToken.Patient.MobileNumber,
                            Age = createdToken.Patient.Age
                        } : null
                };
                // Send appointment confirmation email
                if (!string.IsNullOrWhiteSpace(patient.Email))
                {
                    var emailRequest = new EmailRequest
                    {
                        ToEmail = patient.Email,
                        PatientName = patient.Name,
                        DoctorName = createdToken.Doctor?.Name ?? "Not Assigned",
                        DepartmentName = createdToken.Department?.Name ?? "General",
                        TokenNumber = createdToken.TokenNumber,
                        AppointmentDate = createdToken.CreatedAt ?? DateTime.Now,
                        PriorityLevel = priorityName ?? "Normal",
                        Symptoms = patient.Symptoms
                    };
                    // Send email asynchronously without blocking token generation
                    _ = _emailService.SendAppointmentConfirmationAsync(emailRequest);
                }
                return new ApiResponse<TokenDto> { Success = true, Message = $"Token generated successfully with number: {tokenNumber}", Data = tokenDto };
            }
            catch (Exception ex)
            {
                return new ApiResponse<TokenDto> { Success = false, Message = $"Error: {ex.Message}" };
            }
        }
        #endregion
        #endregion

    }
}

