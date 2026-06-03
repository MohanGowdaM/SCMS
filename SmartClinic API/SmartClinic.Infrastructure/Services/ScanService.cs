using Microsoft.EntityFrameworkCore;
using SmartClinic.Application.DTOs;
using SmartClinic.Application.Interfaces;
using SmartClinic.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Infrastructure.Services
{

    public class ScanService : IScanService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ScanService> _logger;
        public ScanService(ApplicationDbContext context, ILogger<ScanService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ScanQueueDto> GetScanQueueAsync()
        {
            try
            {
                var scanQueueDtos = new ScanQueueDto();
                var scanQueue = await _context.Tokens.Include(p => p.Patient).Where(x => x.ServiceId == 4 && x.CreatedAt >= DateTime.Today).ToListAsync();
                if (scanQueue.Count > 0)
                {
                    _logger.LogInformation($"Found {scanQueue.Count} scan requests in the queue.");
                    var waitingStatus = scanQueue.Where(x => x.StatusId == 1).ToList();
                    foreach (var token in waitingStatus)
                    {
                        var request = await _context.PatientScanRequests.FirstOrDefaultAsync(x => x.TokenId == token.Id);
                        if (request != null)
                        {
                            scanQueueDtos.WaitingScans.Add(new ScanQueueDto.WaitingScanDto
                            {
                                Id = request.Id,
                                TokenId = token.Id,
                                TokenNumber = token.TokenNumber,
                                PatientName = token.Patient?.Name,
                                ScanTypes = request.ScanType,
                                Notes = request.Notes
                            });
                        }
                        else
                        {
                            _logger.LogWarning($"No scan request found for token ID: {token.Id}");
                            continue;
                        }
                    }
                    var holdStatus = scanQueue.Where(x => x.StatusId == 4).ToList();
                    foreach (var token in holdStatus)
                    {
                        var request = await _context.PatientScanRequests.FirstOrDefaultAsync(x => x.TokenId == token.Id);
                        if (request != null)
                        {
                            scanQueueDtos.HoldScans.Add(new ScanQueueDto.HoldScanDto
                            {
                                Id = request.Id,
                                TokenId = token.Id,
                                TokenNumber = token.TokenNumber,
                                PatientName = token.Patient?.Name,
                                ScanTypes = request.ScanType,
                                Notes = request.Notes
                            });
                        }
                        else
                        {
                            _logger.LogWarning($"No scan request found for token ID: {token.Id}");
                            continue;
                        }
                    }
                    var skippedStatus = scanQueue.Where(x => x.StatusId == 5).ToList();
                    foreach (var token in skippedStatus)
                    {
                        var request = await _context.PatientScanRequests.FirstOrDefaultAsync(x => x.TokenId == token.Id);
                        if (request != null)
                        {
                            scanQueueDtos.SkippedScans.Add(new ScanQueueDto.SkippedScanDto
                            {
                                Id = request.Id,
                                TokenId = token.Id,
                                TokenNumber = token.TokenNumber,
                                PatientName = token.Patient?.Name,
                                ScanTypes = request.ScanType,
                                Notes = request.Notes
                            });
                        }
                        else
                        {
                            _logger.LogWarning($"No scan request found for token ID: {token.Id}");
                            continue;
                        }
                    }
                    var next3Scans = scanQueue.Where(x => x.StatusId == 1).OrderBy(x => x.CreatedAt).Take(3).ToList();
                    foreach (var item in next3Scans)
                    {
                        var request = await _context.PatientScanRequests.FirstOrDefaultAsync(x => x.TokenId == item.Id);
                        if (request != null)
                        {
                            scanQueueDtos.Next3Scans.Add(new ScanQueueDto.Next3ScansDto
                            {
                                Id = request.Id,
                                TokenId = item.Id,
                                TokenNumber = item.TokenNumber,
                                PatientName = item.Patient?.Name,
                                ScanTypes = request.ScanType,
                                Notes = request.Notes
                            });
                        }
                        else
                        {
                            _logger.LogWarning($"No scan request found for token ID: {item.Id}");
                            continue;
                        }
                    }
                    return scanQueueDtos;
                }
                else
                {
                    return new ScanQueueDto
                    {
                        WaitingScans = new List<ScanQueueDto.WaitingScanDto>(),
                        HoldScans = new List<ScanQueueDto.HoldScanDto>(),
                        SkippedScans = new List<ScanQueueDto.SkippedScanDto>(),
                        Next3Scans = new List<ScanQueueDto.Next3ScansDto>()
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading scan queue");
                throw new Exception($"Error loading scan queue: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> CompleteScanAsync(int id, int tokenId)
        {
            try
            {
                // Validate Request
                if (id <= 0)
                {
                    return new ApiResponse<bool> { Success = false, Message = "Invalid scan request id" };
                }
                if (tokenId <= 0)
                {
                    return new ApiResponse<bool> { Success = false, Message = "Invalid token id" };
                }
                // Get Scan Request
                var request = await _context.PatientScanRequests.FirstOrDefaultAsync(x => x.Id == id);
                if (request == null)
                {
                    return new ApiResponse<bool> { Success = false, Message = "Scan request not found" };
                }
                // Mark Completed
                request.IsCompleted = true;
                request.CompletedAt = DateTime.Now;
                // Return To Doctor Queue
                var token = await _context.Tokens.FirstOrDefaultAsync(x => x.Id == tokenId && x.IsDeleted == false);
                if (token == null)
                {
                    return new ApiResponse<bool> { Success = false, Message = "Patient token not found" };
                }
                var DoctorReview = await _context.TokenStatuses.FirstOrDefaultAsync(x => x.Name.ToLower() == "WaitingDoctorReview");
                if (DoctorReview == null)
                {
                    return new ApiResponse<bool> { Success = false, Message = "Doctor review status not found in system" };
                }
                // WaitingDoctorReview
                token.StatusId = DoctorReview.Id;
                await _context.SaveChangesAsync();
                return new ApiResponse<bool> { Success = true, Message = "Scan completed and patient returned to doctor" };
            }
            catch (DbUpdateException ex)
            {
                return new ApiResponse<bool> { Success = false, Message = $"Database error: {ex.InnerException?.Message ?? ex.Message}" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool> { Success = false, Message = $"Error completing scan: {ex.Message}" };
            }
        }
        public async Task<ApiResponse<bool>> UpdateScanTokenStatusAsync(UpdateTokenStatusDto request)
        {
            try
            {
                if (request == null)
                {
                    return new ApiResponse<bool> { Success = false, Message = "Invalid request" };
                }
                else
                {
                    if (request.Action == "CALL")
                    {
                        var token = await _context.Tokens.FirstOrDefaultAsync(x => x.Id == request.TokenId);
                        if (token == null)
                        {
                            return new ApiResponse<bool> { Success = false, Message = "Patient token not found" };
                        }
                        else
                        {

                            token.StatusId = 2; // Called
                            token.CalledAt = DateTime.Now;
                            await _context.SaveChangesAsync();
                            return new ApiResponse<bool> { Success = true, Message = "Patient called successfully" };
                        }
                    }
                    else if (request.Action == "HOLD")
                    {
                        var token = await _context.Tokens.FirstOrDefaultAsync(x => x.Id == request.TokenId);
                        if (token == null)
                        {
                            return new ApiResponse<bool> { Success = false, Message = "Patient token not found" };
                        }
                        else
                        {
                            token.StatusId = 4; // Hold
                            await _context.SaveChangesAsync();
                            return new ApiResponse<bool> { Success = true, Message = "Patient put on hold successfully" };
                        }
                    }
                    else if (request.Action == "SKIP")
                    {
                        var token = await _context.Tokens.FirstOrDefaultAsync(x => x.Id == request.TokenId);
                        if (token == null)
                        {
                            return new ApiResponse<bool> { Success = false, Message = "Patient token not found" };
                        }
                        else
                        {
                            token.StatusId = 5; // Skipped
                            await _context.SaveChangesAsync();
                            return new ApiResponse<bool> { Success = true, Message = "Patient skipped successfully" };
                        }
                    }
                    else if (request.Action == "END")
                    {
                        var token = await _context.Tokens.FirstOrDefaultAsync(x => x.Id == request.TokenId);
                        if (token == null)
                        {
                            return new ApiResponse<bool> { Success = false, Message = "Patient token not found" };
                        }
                        else
                        {
                            token.StatusId = 6; // Waiting
                            await _context.SaveChangesAsync();
                            return new ApiResponse<bool> { Success = true, Message = "Patient returned to waiting queue successfully" };
                        }
                    }
                    else if (request.Action == "RECALL")
                    {
                        var token = await _context.Tokens.FirstOrDefaultAsync(x => x.Id == request.TokenId);
                        if (token == null)
                        {
                            return new ApiResponse<bool> { Success = false, Message = "Patient token not found" };
                        }
                        else
                        {
                            token.StatusId = 2; // Recall to Called
                            await _context.SaveChangesAsync();
                            return new ApiResponse<bool> { Success = true, Message = "Patient returned to waiting queue successfully" };
                        }
                    }
                    else
                    {
                        return new ApiResponse<bool> { Success = false, Message = "Invalid action specified" };
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                return new ApiResponse<bool> { Success = false, Message = $"Database error: {ex.InnerException?.Message ?? ex.Message}" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool> { Success = false, Message = $"Error updating token status: {ex.Message}" };
            }
        }
    }
}
