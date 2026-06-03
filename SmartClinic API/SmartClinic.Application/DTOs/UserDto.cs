using SmartClinic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SmartClinic.Application.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? RoleName { get; set; }
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public int? RoleId { get; set; }
        public int? DepartmentId { get; set; }
        public string? ContactNumber { get; set; }
        public bool? IsDeleted { get; set; }        
        public DateTime? CreatedDateTime { get; set; }        
        public string? CreatedBy { get; set; }      
        public DateTime? UpdatedDateTime { get; set; }     
        public string? UpdatedBy { get; set; }        
        public virtual RolesDto? RolesDto { get; set; }
    }
}
