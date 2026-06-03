namespace SmartClinic.Web.Models.SystemAdmin
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? RoleName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; } = null;
        public int? RoleId { get; set; }
        public int? DepartmentId { get; set; }
        public string? ContactNumber { get; set; }
    }
}
