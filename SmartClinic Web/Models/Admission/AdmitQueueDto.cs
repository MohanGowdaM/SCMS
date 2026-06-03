namespace SmartClinic.Web.Models.Admission
{
    public class AdmitQueueDto
    {
        public int Id { get; set; }

        public int TokenId { get; set; }

        public string? TokenNumber { get; set; }

        public string? PatientName { get; set; }

        public string? WardType { get; set; }

        public string? AdmissionReason { get; set; }
    }
}
