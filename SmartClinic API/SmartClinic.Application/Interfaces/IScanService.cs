using SmartClinic.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Application.Interfaces
{
    public interface IScanService
    {
        Task<ScanQueueDto> GetScanQueueAsync();
 Task<ApiResponse<bool>> UpdateScanTokenStatusAsync(UpdateTokenStatusDto request);
        Task<ApiResponse<bool>> CompleteScanAsync(int id,int tokenId);
    }
}
