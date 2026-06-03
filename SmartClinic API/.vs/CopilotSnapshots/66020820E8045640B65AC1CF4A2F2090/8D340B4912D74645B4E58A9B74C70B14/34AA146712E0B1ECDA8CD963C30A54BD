using SmartClinic.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Application.Interfaces
{
    public interface ILabService
    {
        Task<List<LabQueueDto>> GetLabQueueAsync();
        Task<ApiResponse<CompleteLabTestResponse>> CompleteLabTestAsync(CompleteLabTestRequest request);
    }
}
