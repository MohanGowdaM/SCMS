namespace SmartClinic.Web.Models.SystemAdmin
{
    public class DoctorDto
    {
        public int Id { get; set; }        
        public string? Name { get; set; }
        public string? DepartmentName { get; set; }
        public int? DepartmentId { get; set; }
        public string? ContactNumber { get; set; }
    }
}
