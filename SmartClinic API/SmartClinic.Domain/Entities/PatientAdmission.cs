using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Domain.Entities
{
    public class PatientAdmission
    {
        public int Id { get; set; }

        public int TokenId { get; set; }

        public int DoctorId { get; set; }

        public string? AdmissionReason { get; set; }

        public string? WardType { get; set; }

        public string? BedNumber { get; set; }

        public bool IsAdmitted { get; set; }

        public DateTime? AdmittedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual Token? Token { get; set; }

        public virtual Doctor? Doctor { get; set; }
    }
}
