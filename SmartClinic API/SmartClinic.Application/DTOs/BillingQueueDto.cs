using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Application.DTOs
{
    public class BillingQueueDto
    {
        public int Id { get; set; }

        public int TokenId { get; set; }

        public string? TokenNumber { get; set; }

        public string? PatientName { get; set; }

        public decimal? ConsultationFee { get; set; }

        public decimal? LabFee { get; set; }

        public decimal? ScanFee { get; set; }

        public decimal? MedicineFee { get; set; }

        public decimal? TotalAmount { get; set; }
    }
}
