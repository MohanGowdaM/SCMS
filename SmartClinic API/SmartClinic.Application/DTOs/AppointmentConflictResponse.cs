using System;

namespace SmartClinic.Application.DTOs
{
    public class AppointmentConflictResponse
    {
        /// <summary>
        /// Type of conflict detected: NONE, BLOCK, WARNING_30_MIN, WARNING_SAME_DAY
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// User-friendly message describing the conflict
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Details of the conflicting appointment (if any)
        /// </summary>
        public ConflictAppointmentDetails ConflictDetails { get; set; }

        /// <summary>
        /// Indicates if there is a conflict that needs user confirmation
        /// </summary>
        public bool HasConflict { get; set; }
    }

    public class ConflictAppointmentDetails
    {
        /// <summary>
        /// ID of the existing token
        /// </summary>
        public int TokenId { get; set; }

        /// <summary>
        /// Token number (e.g., CAR-001)
        /// </summary>
        public string TokenNumber { get; set; }

        /// <summary>
        /// Scheduled appointment date and time
        /// </summary>
        public DateTime AppointmentDateTime { get; set; }

        /// <summary>
        /// Department name
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// Doctor name (if assigned)
        /// </summary>
        public string DoctorName { get; set; }

        /// <summary>
        /// Current status of the appointment
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// Minutes until the conflicting appointment (negative = past, positive = future)
        /// </summary>
        public int MinutesUntilConflict { get; set; }
    }
}
