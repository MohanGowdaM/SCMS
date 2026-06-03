using System;

namespace SmartClinic.Application.DTOs
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public int SlNo { get; set; }
        public string? TokenNumber { get; set; }
        public string? Uhid { get; set; }
        public string? PatientName { get; set; }
        public string? DoctorName { get; set; }
        public string? DepartmentName { get; set; }
        public string? PriorityName { get; set; }
        public string? StatusName { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}
