using SmartClinic.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Application.Interfaces
{
    public interface IPharmacyService
    {
        Task<List<PharmacyQueueDto>> GetPharmacyQueueAsync();
        Task<ApiResponse<bool>>DispenseMedicineAsync(DispenseMedicineRequest request);
    }
}
