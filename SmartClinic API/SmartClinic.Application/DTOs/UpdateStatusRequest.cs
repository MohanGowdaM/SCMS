namespace SmartClinic.Application.DTOs
{
    public class UpdateStatusRequest
    {
        public int AppointmentId { get; set; }
        public string? Status { get; set; }
    }
}
