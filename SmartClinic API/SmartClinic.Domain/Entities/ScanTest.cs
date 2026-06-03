using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Domain.Entities
{
    public class ScanTest
    {
        public int Id { get; set; }

        public int ScanCategoryId { get; set; }

        public string? TestName {get; set;}

        public virtual ScanCategory? ScanCategory { get; set;}
    }
}
