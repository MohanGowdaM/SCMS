using SmartClinic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SmartClinic.Application.DTOs
{
    public class NotificationDto
    {       
        public int Id { get; set; }
        public int? PatientId { get; set; }        
        public string? Message { get; set; }
        public int? TypeId { get; set; }       
        public string? Status { get; set; }       
        public DateTime? CreatedAt { get; set; }        
        public virtual NotificationTypeDto? NotificationTypeDto { get; set; }
    }
}
