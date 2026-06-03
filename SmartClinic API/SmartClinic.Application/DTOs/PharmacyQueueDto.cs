using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Application.DTOs
{
    public class PharmacyQueueDto
    {
        public int Id { get; set; }

        public int TokenId { get; set; }

        public string? TokenNumber { get; set; }

        public string? PatientName { get; set; }

        public string? PrescriptionNotes { get; set; }
    }
}
