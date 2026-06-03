using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SmartClinic.Domain.Entities
{
    public class LabTest
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string? TestName { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey("CategoryId")]
        public virtual LabTestCategory? LabTestCategory { get; set; }
    }
}
