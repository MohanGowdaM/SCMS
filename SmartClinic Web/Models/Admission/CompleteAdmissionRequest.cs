namespace SmartClinic.Web.Models.Admission
{
    public class CompleteAdmissionRequest
    {
        public int Id { get; set; }

        public int TokenId { get; set; }

        public string? BedNumber { get; set; }
    }
}
