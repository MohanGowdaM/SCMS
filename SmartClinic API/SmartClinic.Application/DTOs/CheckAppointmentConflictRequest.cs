using System;

namespace SmartClinic.Application.DTOs
{
    public class CheckAppointmentConflictRequest
    {
        /// <summary>
        /// Patient ID to check for conflicts
        /// </summary>
        public int PatientId { get; set; }

        /// <summary>
        /// The appointment date and time to check
        /// </summary>
        public DateTime AppointmentDateTime { get; set; }
    }
}
