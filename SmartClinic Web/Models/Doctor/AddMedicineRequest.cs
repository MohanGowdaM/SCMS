namespace SmartClinic.Web.Models.Doctor
{
    public class AddMedicineRequest
    {
        public string? MedicineName { get; set; }

        public string? Strength { get; set; }

        public string? MedicineType { get; set; }

        public decimal? Price { get; set; }
    }
}
