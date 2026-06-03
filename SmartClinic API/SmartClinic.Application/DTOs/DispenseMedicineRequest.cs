using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Application.DTOs
{
    public class DispenseMedicineRequest
    {
        public int TokenId { get; set; }

        public decimal MedicineFee { get; set; }
    }
}
