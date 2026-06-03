namespace SmartClinic.Web.Models.Doctor
{
    public class SaveConsultationRequest
    {
        public int TokenId { get; set; }

        public int DoctorId { get; set; }

        public string? Diagnosis { get; set; }

        public string? PrescriptionNotes { get; set; }
        public List<PrescriptionMedicineDto>? Medicines { get; set; }
    }
}
