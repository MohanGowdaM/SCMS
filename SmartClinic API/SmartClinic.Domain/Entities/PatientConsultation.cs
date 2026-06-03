using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Domain.Entities
{
    public class PatientConsultation
    {
        public int Id { get; set; }

        public int TokenId { get; set; }

        public int DoctorId { get; set; }

        public string? Diagnosis { get; set; }

        public string? PrescriptionNotes { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual Token? Token { get; set; }

        public virtual Doctor? Doctor { get; set; }

        public virtual ICollection<PatientPrescriptionMedicine>? PrescriptionMedicines {get; set;}
    }
}
