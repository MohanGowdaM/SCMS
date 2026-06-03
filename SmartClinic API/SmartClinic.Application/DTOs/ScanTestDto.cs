using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Application.DTOs
{
    public class ScanTestDto
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public string? TestName { get; set;}
    }
}
