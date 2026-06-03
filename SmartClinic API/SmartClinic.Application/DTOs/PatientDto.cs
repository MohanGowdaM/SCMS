using SmartClinic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SmartClinic.Application.DTOs
{
    public class PatientDto
    {

        public int Id { get; set; }       
        public string? Name { get; set; }       
        public DateOnly? Dob { get; set; }       
        public string? Gender { get; set; }       
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
        public int? Age { get; set; }
        public string? Symptoms { get; set; }
        public string? Uhid { get; set; }       
        public DateTime? CreatedDateTime { get; set; }       
        public string? CreatedBy { get; set; }        
        public DateTime? UpdatedDateTime { get; set; }        
        public string? UpdatedBy { get; set; }        
        public virtual ICollection<TokenDto> Tokens { get; set; } = new List<TokenDto>();
    }
}
