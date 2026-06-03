using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Application.DTOs
{
    public class PaymentRequest
    {
        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public decimal ConsultationFee { get; set; }

        public string? PaymentMode { get; set; }

        public string? Notes { get; set; }
    }
}
