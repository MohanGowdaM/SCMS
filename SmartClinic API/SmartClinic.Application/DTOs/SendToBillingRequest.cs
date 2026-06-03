using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Application.DTOs
{
    public class SendToBillingRequest
    {
        public int TokenId { get; set; }

        public int DoctorId { get; set; }

        public decimal? ConsultationFee { get; set; }

        public decimal? LabFee { get; set; }

        public decimal? ScanFee { get; set; }

        public decimal? MedicineFee { get; set; }

        public string? Notes { get; set; }
    }
}
