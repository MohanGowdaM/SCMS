using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Domain.Entities
{
    public class ScanCategory
    {
        public int Id { get; set; }

        public string? CategoryName { get; set; }

        public virtual ICollection<ScanTest>? ScanTests { get; set; }
    }
}
