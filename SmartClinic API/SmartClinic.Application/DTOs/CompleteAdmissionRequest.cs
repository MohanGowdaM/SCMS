using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Application.DTOs
{
    public class CompleteAdmissionRequest
    {
        public int Id { get; set; }

        public int TokenId { get; set; }

        public string? BedNumber { get; set; }
    }
}
