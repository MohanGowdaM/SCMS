using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartClinic.Application.DTOs;
using SmartClinic.Application.Interfaces;

namespace SmartClinic.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }
       

        [HttpPost("SavePatient")]
        public async Task<IActionResult> SavePatient(PatientDto dto)
        {
            var result = await _patientService.AddPatientAsync(dto);

            if (result==null)
            {
                return BadRequest(new
                {
                    Success = false,

                    Message = "Failed to save patient"
                });
            }

            return Ok(new
            {
                Success = true,

                Message = "Patient saved successfully",

                Data = result
            });
        }

        [HttpGet("GetPatientDetails/{patientId}")]
        public async Task<IActionResult> GetPatientDetails(int patientId)
        {
            var patient = await _patientService.GetPatientDetailsAsync(patientId);

            if (patient == null)
            {
                return NotFound(new
                {
                    Message = "Patient not found"
                });
            }

            return Ok(new
            {
                Message = "Patient details retrieved successfully",
                Data = patient
            });
        }

        [HttpGet("GetAllPatientDetails")]
        public async Task<IActionResult> GetAllPatientDetails()
        {
            var patients = await _patientService.GetAllPatientDetailsAsync();

            if (patients == null || patients.Count == 0)
            {
                return Ok(new
                {
                    Message = "No patients found",
                    Data = new List<PatientDto>()
                });
            }

            return Ok(new
            {
                Message = "All patients retrieved successfully",
                Data = patients
            });
        }

        [HttpPut("UpdatePatient")]
        public async Task<IActionResult> UpdatePatient(PatientDto dto)
        {
            if (dto.Id <= 0)
            {
                return BadRequest(new
                {
                    Message = "Invalid patient ID"
                });
            }

            var result = await _patientService.UpdatePatientAsync(dto);

            if (!result)
            {
                return BadRequest(new
                {
                    Message = "Failed to update patient. Patient may not exist or has been deleted."
                });
            }

            return Ok(new
            {
                Message = "Patient updated successfully"
            });
        }

        [HttpDelete("DeletePatient/{patientId}")]
        public async Task<IActionResult> DeletePatient(int patientId)
        {
            if (patientId <= 0)
            {
                return BadRequest(new
                {
                    Message = "Invalid patient ID"
                });
            }

            var result = await _patientService.DeletePatientAsync(patientId);

            if (!result)
            {
                return BadRequest(new
                {
                    Message = "Failed to delete patient. Patient may not exist or has already been deleted."
                });
            }

            return Ok(new
            {
                Message = "Patient deleted successfully"
            });
        }

        [HttpGet("ViewFullDetails/{patientId}")]
        public async Task<IActionResult> ViewFullDetails(int patientId)
        {
            if (patientId <= 0)
            {
                return BadRequest(new
                {
                    Message = "Invalid patient ID"
                });
            }

            var patient = await _patientService.ViewFullDetailsAsync(patientId);

            if (patient == null)
            {
                return NotFound(new
                {
                    Message = "Patient not found or has been deleted"
                });
            }

            return Ok(new
            {
                Message = "Patient full details retrieved successfully",
                Data = patient
            });
        }

        [HttpGet("SearchPatients")]
        public async Task<IActionResult> SearchPatients(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return BadRequest(new
                {
                    Message = "Search term cannot be empty"
                });
            }

            var patients = await _patientService.SearchPatientsAsync(search);

            if (patients == null || patients.Count == 0)
            {
                return Ok(new
                {
                    Message = "No patients found matching the search criteria",
                    Data = new List<PatientDto>()
                });
            }

            return Ok(new
            {
                Message = $"Found {patients.Count} patient(s) matching the search criteria",
                Data = patients
            });
        }
    }
}

