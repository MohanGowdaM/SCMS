namespace SmartClinic.Web.Models.Doctor
{
    public class SendToScanRequest
    {
        public int TokenId { get; set; }

        public int DoctorId { get; set; }

        public List<string>? ScanTypes { get; set; }

        public string? Notes { get; set; }
    }
}
