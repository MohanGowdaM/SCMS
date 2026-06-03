using SmartClinic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SmartClinic.Application.DTOs
{
    public class DoctorDto
    {        
        public int Id { get; set; }        
        public string? Name { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public string? ContactNumber { get; set; }
        public bool? IsAvailable { get; set; }
        public int? ConsultationTimeInMin { get; set; }        
        public DateTime? CreatedDateTime { get; set; }       
        public string? CreatedBy { get; set; }        
        public DateTime? UpdatedDateTime { get; set; }      
        public string? UpdatedBy { get; set; }        
        public virtual DepartmentDto? DepartmentDto { get; set; }       
        public virtual ICollection<TokenDto> TokensDto { get; set; } = new List<TokenDto>();
    }
}
