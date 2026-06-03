namespace SmartClinic.Web.Models.Account
{
    public class LoginResponseViewModel
    {
        public bool IsSuccess { get; set; }

        public string Token { get; set; }
            = string.Empty;

        public string Username { get; set; }
            = string.Empty;

        public string Role { get; set; }
            = string.Empty;

        public int UserId { get; set; }

        public string Message { get; set; }
            = string.Empty;

        public int DoctorId { get; set; }
    }
}
