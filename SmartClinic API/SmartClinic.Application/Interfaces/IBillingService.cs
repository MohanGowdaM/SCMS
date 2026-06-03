using SmartClinic.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Application.Interfaces
{
    public interface IBillingService
    {
        Task<List<BillingQueueDto>> GetBillingQueueAsync();

        Task<ApiResponse<bool>> CompletePaymentAsync(CompletePaymentRequest request);

        Task<ApiResponse<bool>> SendToPharmacyAsync(int tokenId);
        Task<ApiResponse<bool>>CompleteVisitAsync(int tokenId);
    }
}
