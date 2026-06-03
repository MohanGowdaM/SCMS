using System;

namespace SmartClinic.Application.DTOs
{
    public class EmailRequest
    {
        public string? ToEmail { get; set; }
        public string? PatientName { get; set; }
        public string? DoctorName { get; set; }
        public string? DepartmentName { get; set; }
        public string? TokenNumber { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string? PriorityLevel { get; set; }
        public string? Symptoms { get; set; }
    }
}
