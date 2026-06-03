using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Domain.Entities
{
    public class Medicine
    {
        public int Id { get; set; }

        public string? MedicineName { get; set; }

        public string? Strength { get; set; }

        public string? MedicineType { get; set; }

        public decimal? Price { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
