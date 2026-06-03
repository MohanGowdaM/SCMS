namespace SmartClinic.Web.Models.Appointment
{
    public class UpdateQueueRequest
    {
        public string? Action { get; set; }

        public int DoctorId { get; set; }

        public int? TokenId { get; set; }
    }
}
