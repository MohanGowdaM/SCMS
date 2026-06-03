namespace SmartClinic.Web.Models.Appointment
{
    public class CheckAppointmentConflictRequest
    {
        public int PatientId { get;set;}

        public DateTime AppointmentDateTime {get;set;}
    }
}
