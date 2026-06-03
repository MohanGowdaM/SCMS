using Microsoft.EntityFrameworkCore;
using SmartClinic.Application.DTOs;
using SmartClinic.Application.Interfaces;
using SmartClinic.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Infrastructure.Services
{
    public class BillingService : IBillingService
    {
        private readonly ApplicationDbContext _context;

        public BillingService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<BillingQueueDto>> GetBillingQueueAsync()
        {
            try
            {
                var billingQueue = await _context.PatientBillingRequests.Where(x => x.IsPaid == false)
                    .Select(x => new BillingQueueDto
                    {
                        Id = x.Id,

                        TokenId = x.TokenId,

                        TokenNumber = x.Token != null ? x.Token.TokenNumber : string.Empty,

                        PatientName = x.Token != null && x.Token.Patient != null ? x.Token.Patient.Name : string.Empty,

                        ConsultationFee = x.ConsultationFee,

                        LabFee = x.LabFee,

                        ScanFee = x.ScanFee,

                        MedicineFee = x.MedicineFee,

                        TotalAmount = x.TotalAmount
                    }).ToListAsync();

                return billingQueue;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading billing queue: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> CompletePaymentAsync(CompletePaymentRequest request)
        {
            try
            {
                var billing = await _context.PatientBillingRequests.FirstOrDefaultAsync(x => x.Id == request.BillingId);

                if (billing == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Billing record not found"
                    };
                }

                billing.IsPaid = true;

                billing.PaidAt = DateTime.Now;

                var VisitComplete = await _context.TokenStatuses.FirstOrDefaultAsync(x => x.Name.ToLower() == "Paid");

                if (VisitComplete == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Completed status not found in system"
                    };
                }

                var token = await _context.Tokens.FirstOrDefaultAsync(x => x.Id == request.TokenId);

                if (token != null)
                {
                    token.StatusId = VisitComplete.Id; // VisitCompleted
                }

                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Payment completed successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ApiResponse<bool>> SendToPharmacyAsync(int tokenId)
        {
            try
            {
                var SendToPharmacy = await _context.TokenStatuses.FirstOrDefaultAsync(x => x.Name.ToLower() == "SentToPharmacy");

                if (SendToPharmacy == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "SendToPharmacy status not found in system"
                    };
                }

                var token = await _context.Tokens.FirstOrDefaultAsync(x => x.Id == tokenId);

                if (token == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Patient token not found"
                    };
                }

                token.StatusId = SendToPharmacy.Id;

                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message =
                        "Patient sent to pharmacy"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message =
                        ex.Message
                };
            }
        }

        public async Task<ApiResponse<bool>> CompleteVisitAsync(int tokenId)
        {
            try
            {
                var Completed = await _context.TokenStatuses.FirstOrDefaultAsync(x => x.Name.ToLower() == "Completed");

                if (Completed == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Completed status not found in system"
                    };
                }

                var token = await _context.Tokens.FirstOrDefaultAsync(x => x.Id == tokenId);

                if (token == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Patient token not found"
                    };
                }

                token.StatusId = Completed.Id;

                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Visit completed successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message =
                        ex.Message
                };
            }
        }
    }
}
