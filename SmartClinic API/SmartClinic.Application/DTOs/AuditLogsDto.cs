using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SmartClinic.Application.DTOs
{
    public class AuditLogsDto
    {        
        public int Id { get; set; }
        public int? UserId { get; set; }       
        public string? Action { get; set; }       
        public DateTime? Timestamp { get; set; }
    }
}
