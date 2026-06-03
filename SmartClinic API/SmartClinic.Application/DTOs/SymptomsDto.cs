using SmartClinic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SmartClinic.Application.DTOs
{
    public class SymptomsDto
    {       
        public int Id { get; set; }        
        public string? Name { get; set; }
        public int? Weight { get; set; }
        public bool? IsDeleted { get; set; }        
        public virtual ICollection<TokenSymptomDto> TokenSymptomsDto { get; set; } = new List<TokenSymptomDto>();
    }
}
