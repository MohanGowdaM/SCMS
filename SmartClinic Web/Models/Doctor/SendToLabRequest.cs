namespace SmartClinic.Web.Models.Doctor
{
    public class SendToLabRequest
    {
        public int TokenId { get; set; }

        public int DoctorId { get; set; }

        public List<string>? LabTests { get; set; }

        public string? Notes { get; set; }
    }
}
