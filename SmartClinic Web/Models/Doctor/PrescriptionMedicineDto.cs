namespace SmartClinic.Web.Models.Doctor
{
    public class PrescriptionMedicineDto
    {
        public int MedicineId { get; set; }

        public string? MedicineName { get; set; }

        public int Quantity { get; set; }

        public string? Dosage { get; set; }

        public string? Timing { get; set; }

        public string? Food { get; set; }

        public int Days { get; set; }
    }
}
