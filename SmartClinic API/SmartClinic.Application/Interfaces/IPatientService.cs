using SmartClinic.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Application.Interfaces
{
    public interface IPatientService
    {
        //Task<bool> AddPatientAsync(PatientDto dto);

        Task<PatientDto> AddPatientAsync(PatientDto dto);
        Task<PatientDto> GetPatientDetailsAsync(int patientId);
        Task<List<PatientDto>> GetAllPatientDetailsAsync();
        Task<bool> UpdatePatientAsync(PatientDto dto);
        Task<bool> DeletePatientAsync(int patientId);
        Task<PatientDto> ViewFullDetailsAsync(int patientId);
        Task<List<PatientDto>> SearchPatientsAsync(string searchTerm);
    }
}
