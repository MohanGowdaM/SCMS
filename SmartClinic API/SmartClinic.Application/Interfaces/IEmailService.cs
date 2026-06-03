using System.Threading.Tasks;
using SmartClinic.Application.DTOs;

namespace SmartClinic.Application.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendAppointmentConfirmationAsync(EmailRequest request);
    }
}
