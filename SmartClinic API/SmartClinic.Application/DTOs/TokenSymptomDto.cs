using SmartClinic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SmartClinic.Application.DTOs
{
    public class TokenSymptomDto
    {        
        public int Id { get; set; }
        public int? TokenId { get; set; }
        public int? SymptomId { get; set; }       
        public virtual SymptomsDto? SymptomsDto { get; set; }        
        public virtual TokenDto? TokenDto { get; set; }
    }
}
