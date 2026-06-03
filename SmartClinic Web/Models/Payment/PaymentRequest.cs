namespace SmartClinic.Web.Models.Payment
{
    public class PaymentRequest
    {
        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public decimal ConsultationFee { get; set; }

        public string? PaymentMode { get; set; }

        public string? Notes { get; set; }
    }
}
