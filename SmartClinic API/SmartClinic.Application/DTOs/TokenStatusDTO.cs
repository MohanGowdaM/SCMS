using SmartClinic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SmartClinic.Application.DTOs
{
    public class TokenStatusDTO
    {        
        public int Id { get; set; }        
        public string? Name { get; set; }        
        public virtual ICollection<TokenDto> TokenDto { get; set; } = new List<TokenDto>();
    }
}
