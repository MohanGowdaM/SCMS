using Microsoft.EntityFrameworkCore;
using SmartClinic.Application.DTOs;
using SmartClinic.Application.Interfaces;
using SmartClinic.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Infrastructure.Services
{

    public class ScanService :IScanService
    {
        private readonly ApplicationDbContext _context;

        public ScanService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ScanQueueDto>> GetScanQueueAsync()
        {
            try
            {
                var scanQueue =
                    await _context
                        .PatientScanRequests
                        .Where(x =>
                            x.IsCompleted == false
                        )
                        .Select(x =>
                            new ScanQueueDto
                            {
                                Id = x.Id,

                                TokenId =
                                    x.TokenId,

                                TokenNumber =
                                    x.Token != null
                                        ? x.Token.TokenNumber
                                        : string.Empty,

                                PatientName =
                                    x.Token != null
                                    && x.Token.Patient != null
                                        ? x.Token.Patient.Name
                                        : string.Empty,

                                ScanTypes =
                                    x.ScanType,

                                Notes =
                                    x.Notes
                            }).ToListAsync();

                return scanQueue;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Error loading scan queue: {ex.Message}"
                );
            }
        }

        public async Task<ApiResponse<bool>> CompleteScanAsync(
    int id,
    int tokenId)
        {
            try
            {
                // Validate Request
                if (id <= 0)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message =
                            "Invalid scan request id"
                    };
                }

                if (tokenId <= 0)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message =
                            "Invalid token id"
                    };
                }

                // Get Scan Request
                var request =
                    await _context
                        .PatientScanRequests
                        .FirstOrDefaultAsync(
                            x =>
                                x.Id == id
                        );

                if (request == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message =
                            "Scan request not found"
                    };
                }

                // Mark Completed
                request.IsCompleted =
                    true;

                request.CompletedAt =
                    DateTime.Now;

                // Return To Doctor Queue
                var token =
                    await _context
                        .Tokens
                        .FirstOrDefaultAsync(
                            x =>
                                x.Id ==
                                tokenId
                                && x.IsDeleted == false
                        );

                if (token == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message =
                            "Patient token not found"
                    };
                }

                var DoctorReview = await _context.TokenStatuses
                    .FirstOrDefaultAsync(x => x.Name.ToLower() == "WaitingDoctorReview");

                if (DoctorReview == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Doctor review status not found in system"
                    };
                }

                // WaitingDoctorReview
                token.StatusId = DoctorReview.Id;

                await _context
                    .SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message =
                        "Scan completed and patient returned to doctor"
                };
            }
            catch (DbUpdateException ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message =
                        $"Database error: {ex.InnerException?.Message ?? ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message =
                        $"Error completing scan: {ex.Message}"
                };
            }
        }
    }
}
