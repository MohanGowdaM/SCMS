using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Application.DTOs
{
    public class SendToLabRequest
    {
        public int TokenId { get; set; }

        public int DoctorId { get; set; }

        public List<string>? LabTests { get; set; }

        public string? Notes { get; set; }
    }
}
