using SmartClinic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SmartClinic.Application.DTOs
{
    public class NotificationTypeDto
    {        
        public int Id { get; set; }        
        public string? Name { get; set; }       
        public string? MediaType { get; set; }    
        public virtual ICollection<NotificationDto> NotificationDto { get; set; } = new List<NotificationDto>();
    }
}
