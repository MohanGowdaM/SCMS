using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Domain.Entities
{
    public class PatientScanRequest
    {
        public int Id { get; set; }

        public int TokenId { get; set; }

        public int DoctorId { get; set; }

        public string? ScanType { get; set; }

        public string? Notes { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime? CompletedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual Token? Token { get; set; }

        public virtual Doctor? Doctor { get; set; }
    }
}
