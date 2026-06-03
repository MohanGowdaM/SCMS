using SmartClinic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SmartClinic.Application.DTOs
{
    public class RolesDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Discription { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public bool? IsDeleted { get; set; }
        public virtual ICollection<UserDto> Users { get; set; } = new List<UserDto>();
    }
}
