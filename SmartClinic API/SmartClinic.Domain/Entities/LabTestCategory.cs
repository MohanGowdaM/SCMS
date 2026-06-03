using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Domain.Entities
{
    public class LabTestCategory
    {
        public int Id { get; set; }

        public string? CategoryName { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<LabTest>? LabTests { get; set; }
    }
}
