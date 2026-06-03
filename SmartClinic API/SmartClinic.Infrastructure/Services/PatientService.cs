using SmartClinic.Application.DTOs;
using SmartClinic.Application.Interfaces;
using SmartClinic.Domain.Entities;
using SmartClinic.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartClinic.Infrastructure.Services
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _context;

        public PatientService(ApplicationDbContext context)
        {
            _context = context;
        }       


        public async Task<PatientDto>AddPatientAsync(PatientDto dto)
        {
            try
            {
                var patient = new Patient
                    {
                        Name =
                            dto.Name,

                        Gender =
                            dto.Gender,

                        MobileNumber =
                            dto.MobileNumber,

                        Dob =
                            dto.Dob,

                        Age =
                            dto.Age,

                        Email =
                            dto.Email,

                        IsDeleted =
                            false,

                        CreatedDateTime =
                            DateTime.Now,

                        CreatedBy =
                            "System"
                    };

                await _context
                    .Patients
                    .AddAsync(
                        patient);

                await _context
                    .SaveChangesAsync();

                // Generate UHID
                patient.Uhid =
                    $"SC{DateTime.Now:yyMM}{patient.Id:D4}";

                await _context
                    .SaveChangesAsync();

                // Return patient details
                return new PatientDto
                {
                    Id =
                        patient.Id,

                    Uhid =
                        patient.Uhid,

                    Name =
                        patient.Name,

                    MobileNumber =
                        patient.MobileNumber,

                    Age =
                        patient.Age,

                    Gender =
                        patient.Gender,

                    Email =
                        patient.Email,

                    Dob =
                        patient.Dob
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<PatientDto> GetPatientDetailsAsync(int patientId)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(patientId);

                if (patient == null || patient.IsDeleted)
                {
                    return null;
                }

                var patientDto = new PatientDto
                {
                    Id = patient.Id,
                    Name = patient.Name,
                    Dob = patient.Dob,
                    Gender = patient.Gender,
                    MobileNumber = patient.MobileNumber,
                    Age = patient.Age,
                    Symptoms = patient.Symptoms,
                    Uhid = patient.Uhid,
                    CreatedDateTime = patient.CreatedDateTime,
                    CreatedBy = patient.CreatedBy,
                    UpdatedDateTime = patient.UpdatedDateTime,
                    UpdatedBy = patient.UpdatedBy
                };

                return patientDto;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<PatientDto>> GetAllPatientDetailsAsync()
        {
            try
            {
                var patients = await _context.Patients.Where(p => !p.IsDeleted).ToListAsync();

                var patientDtos = patients.Select(patient => new PatientDto
                {
                    Id = patient.Id,
                    Name = patient.Name,
                    Dob = patient.Dob,
                    Gender = patient.Gender,
                    MobileNumber = patient.MobileNumber,
                    Age = patient.Age,
                    Symptoms = patient.Symptoms,
                    Uhid = patient.Uhid,
                    CreatedDateTime = patient.CreatedDateTime,
                    CreatedBy = patient.CreatedBy,
                    UpdatedDateTime = patient.UpdatedDateTime,
                    UpdatedBy = patient.UpdatedBy
                }).OrderByDescending(x=>x.Id).ToList();

                return patientDtos;
            }
            catch
            {
                return new List<PatientDto>();
            }
        }

        public async Task<bool> UpdatePatientAsync(PatientDto dto)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(dto.Id);

                if (patient == null || patient.IsDeleted)
                {
                    return false;
                }

                patient.Name = dto.Name;
                patient.Dob = dto.Dob;
                patient.Gender = dto.Gender;
                patient.MobileNumber = dto.MobileNumber;
                patient.Age = dto.Age;
                patient.Symptoms = dto.Symptoms;
                patient.UpdatedDateTime = DateTime.Now;
                patient.UpdatedBy = "System";

                _context.Patients.Update(patient);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeletePatientAsync(int patientId)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(patientId);

                if (patient == null || patient.IsDeleted)
                {
                    return false;
                }

                patient.IsDeleted = true;
                patient.UpdatedDateTime = DateTime.Now;
                patient.UpdatedBy = "System";

                _context.Patients.Update(patient);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<PatientDto> ViewFullDetailsAsync(int patientId)
        {
            try
            {
                var patient = await _context.Patients
                    .Include(p => p.Tokens)
                        .ThenInclude(t => t.Department)
                    .Include(p => p.Tokens)
                        .ThenInclude(t => t.Doctor)
                    .Include(p => p.Tokens)
                        .ThenInclude(t => t.Status)
                    .FirstOrDefaultAsync(p => p.Id == patientId && !p.IsDeleted);

                if (patient == null)
                {
                    return null;
                }

                var patientDto = new PatientDto
                {
                    Id = patient.Id,
                    Name = patient.Name,
                    Dob = patient.Dob,
                    Gender = patient.Gender,
                    MobileNumber = patient.MobileNumber,
                    Age = patient.Age,
                    Symptoms = patient.Symptoms,
                    Uhid = patient.Uhid,
                    CreatedDateTime = patient.CreatedDateTime,
                    CreatedBy = patient.CreatedBy,
                    UpdatedDateTime = patient.UpdatedDateTime,
                    UpdatedBy = patient.UpdatedBy,
                    Tokens = patient.Tokens.Select(token => new TokenDto
                    {
                        Id = token.Id,
                        TokenNumber = token.TokenNumber,
                        PatientId = token.PatientId,
                        DepartmentId = token.DepartmentId,
                        DoctorId = token.DoctorId,
                        Priority = token.Priority,
                        StatusId = token.StatusId,
                        CreatedAt = token.CreatedAt,
                        CalledAt = token.CalledAt,
                        CompletedAt = token.CompletedAt,
                        DepartmentDto = token.Department != null ? new DepartmentDto
                        {
                            Id = token.Department.Id,
                            Name = token.Department.Name
                        } : null,
                        DoctorDto = token.Doctor != null ? new DoctorDto
                        {
                            Id = token.Doctor.Id,
                            Name = token.Doctor.Name
                        } : null,
                        TokenStatusDto = token.Status != null ? new TokenStatusDTO
                        {
                            Id = token.Status.Id,
                            Name = token.Status.Name
                        } : null
                    }).ToList()
                };

                return patientDto;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<PatientDto>> SearchPatientsAsync(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return new List<PatientDto>();
                }

                var patients = await _context.Patients
                    .Where(p => !p.IsDeleted &&
                        (p.Uhid.Contains(searchTerm) ||
                         p.Name.Contains(searchTerm) ||
                         p.MobileNumber.Contains(searchTerm)))
                    .Take(20)
                    .ToListAsync();

                var patientDtos = patients.Select(patient => new PatientDto
                {
                    Id = patient.Id,
                    Name = patient.Name,
                    Dob = patient.Dob,
                    Gender = patient.Gender,
                    MobileNumber = patient.MobileNumber,
                    Age = patient.Age,
                    Symptoms = patient.Symptoms,
                    Uhid = patient.Uhid,
                    CreatedDateTime = patient.CreatedDateTime,
                    CreatedBy = patient.CreatedBy,
                    UpdatedDateTime = patient.UpdatedDateTime,
                    UpdatedBy = patient.UpdatedBy
                }).ToList();

                return patientDtos;
            }
            catch
            {
                return new List<PatientDto>();
            }
        }        
    }
}


