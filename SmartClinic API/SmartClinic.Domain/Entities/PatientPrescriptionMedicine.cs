using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Domain.Entities
{
    public class PatientPrescriptionMedicine
    {
        public int Id { get; set; }

        public int ConsultationId { get; set; }

        public int MedicineId { get; set; }

        public string? MedicineName { get; set; }

        public int Quantity { get; set; }

        public string? Dosage { get; set; }

        public string? Timing { get; set; }

        public string? FoodInstruction { get; set; }

        public int Days { get; set; }

        public bool IsDispensed { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual PatientConsultation? Consultation { get; set; }
    }
}
