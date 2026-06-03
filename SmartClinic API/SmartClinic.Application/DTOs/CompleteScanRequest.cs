using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Application.DTOs
{
    public class CompleteScanRequest
    {
        public int Id { get; set; }

        public int TokenId { get; set; }
    }
}
