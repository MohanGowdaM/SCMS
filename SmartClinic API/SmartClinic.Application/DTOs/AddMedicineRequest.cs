using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Application.DTOs
{
    public class AddMedicineRequest
    {
        public string? MedicineName { get; set; }

        public string? Strength { get; set; }

        public string? MedicineType { get; set; }

        public decimal? Price { get; set; }
    }
}
