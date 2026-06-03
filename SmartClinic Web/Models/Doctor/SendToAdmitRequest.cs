namespace SmartClinic.Web.Models.Doctor
{
    public class SendToAdmitRequest
    {
        public int TokenId { get; set; }

        public int DoctorId { get; set; }

        public string? AdmissionReason { get; set; }

        public string? WardType { get; set; }
    }
}
