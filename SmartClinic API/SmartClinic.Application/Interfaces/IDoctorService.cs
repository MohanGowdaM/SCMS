using SmartClinic.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Application.Interfaces
{
    public interface IDoctorService
    {
        Task<DoctorQueueResponseDto> GetDoctorQueueAsync(int doctorId);
        Task<ApiResponse<DoctorQueueResponseDto>> UpdateQueueStatusAsync(UpdateQueueRequest request);
        Task<ApiResponse<bool>> SaveConsultationAsync(SaveConsultationRequest request);

        Task<ApiResponse<bool>> SendToLabAsync(SendToLabRequest request);

        Task<ApiResponse<bool>> SendToScanAsync(SendToScanRequest request);

        Task<ApiResponse<bool>> SendToBillingAsync(SendToBillingRequest request);

        Task<ApiResponse<bool>>SendToAdmitAsync(SendToAdmitRequest request);

        Task<List<MedicineDto>> GetMedicinesAsync();

        Task<ApiResponse<int>>AddMedicineAsync(AddMedicineRequest request);

        Task<List<LabTestDto>> GetLabTestsAsync();

        Task<List<ScanTestDto>> GetScanTestsAsync();
    }
}
