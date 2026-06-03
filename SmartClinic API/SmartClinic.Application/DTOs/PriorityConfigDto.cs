using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartClinic.Application.DTOs
{
    public class PriorityConfigDto
    {        
        public int Id { get; set; }       
        public string? Name { get; set; }
        public int? PriorityLevel { get; set; }
        public int? MinWeight { get; set; }
        public int? MaxWeight { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
