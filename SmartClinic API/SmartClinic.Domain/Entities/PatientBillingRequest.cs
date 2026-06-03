using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Domain.Entities
{
    public class PatientBillingRequest
    {
        public int Id { get; set; }

        public int TokenId { get; set; }

        public int DoctorId { get; set; }

        public decimal? ConsultationFee { get; set; }

        public decimal? LabFee { get; set; }

        public decimal? ScanFee { get; set; }

        public decimal? MedicineFee { get; set; }

        public decimal? TotalAmount { get; set; }

        public string? Notes { get; set; }

        public bool IsPaid { get; set; }

        public DateTime? PaidAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public int PatientId { get; set; }

        public virtual Patient? Patient { get; set; }

        public virtual Token? Token { get; set; }

        public virtual Doctor? Doctor { get; set; }
    }
}
