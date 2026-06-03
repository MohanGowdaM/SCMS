using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Application.DTOs
{
    public class ScanQueueDto
    {
        public int Id { get; set; }

        public int TokenId { get; set; }

        public string? TokenNumber { get; set; }

        public string? PatientName { get; set; }

        public string? ScanTypes { get; set; }

        public string? Notes { get; set; }
    }
}
