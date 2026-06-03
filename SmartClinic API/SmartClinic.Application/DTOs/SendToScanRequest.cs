using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Application.DTOs
{
    public class SendToScanRequest
    {
        public int TokenId { get; set; }

        public int DoctorId { get; set; }

        public List<string>? ScanTypes { get; set; }

        public string? Notes { get; set; }
    }
}
