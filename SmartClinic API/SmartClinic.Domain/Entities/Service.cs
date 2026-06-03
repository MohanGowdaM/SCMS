using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SmartClinic.Domain.Entities
{
    [Table("Service")]
    public class Service
    {
        public int Id { get; set; }
        public string? Name { get; set; }    
        public bool IsDeleted { get; set; }
        public ICollection<Token> Tokens { get; set; } = new List<Token>();
    }
}
