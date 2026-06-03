using System.Threading.Tasks;

namespace SmartClinic.Application.Interfaces
{
    public interface ITokenSequenceService
    {
        Task<string> GenerateTokenNumberAsync(int departmentId, string departmentName);
    }
}
