namespace SmartClinic.Web.Models.Lab
{
    public class LabQueueDto
    {
        public int Id { get; set; }

        public int TokenId { get; set; }

        public string? TokenNumber { get; set; }

        public string? PatientName { get; set; }

        public string? LabTests { get; set; }

        public string? Notes { get; set; }
    }
}
