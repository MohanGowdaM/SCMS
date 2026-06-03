using Microsoft.EntityFrameworkCore;
using SmartClinic.Application.DTOs;
using SmartClinic.Application.Interfaces;
using SmartClinic.Domain.Entities;
using SmartClinic.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Application.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly ApplicationDbContext _context;

        public DoctorService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<DoctorQueueResponseDto> GetDoctorQueueAsync(int doctorId)
        {
            try
            {


                var doctor = await _context.Doctors.FirstOrDefaultAsync(x => x.Id == doctorId);

                if (doctor == null)
                    return null;

                // Waiting Queue
                var waitingQueue = await _context.Tokens
                    .Include(x => x.Patient)
                    .Include(x => x.Status)
                    .Where(x => x.DoctorId == doctorId && x.Status.Id == 1 && x.ServiceId == 2 && x.IsDeleted == false)
                    .OrderBy(x => x.CreatedAt)
                    .Select(x => new QueuePatientDto
                    {
                        Id = x.Id,
                        TokenNumber = x.TokenNumber,
                        PatientName = x.Patient.Name,
                        PriorityName = _context.PriorityConfigs
                            .Where(p => p.Id == x.Id)
                            .Select(p => p.Name)
                            .FirstOrDefault()
                    }).ToListAsync();

                // Current Called Ticket
                var currentTicket = await _context.Tokens
                    .Include(x => x.Patient)
                    .Include(x => x.Status)
                    .Where(x => x.DoctorId == doctorId
                             && x.Status.Id == 2 && x.ServiceId == 2 && x.IsDeleted == false)
                    .OrderByDescending(x => x.CreatedAt)
                    .Select(x => new QueuePatientDto
                    {
                        Id = x.Id,
                        TokenNumber = x.TokenNumber,
                        PatientName = x.Patient.Name,
                        PriorityName = _context.PriorityConfigs
                            .Where(p => p.Id == x.Id)
                            .Select(p => p.Name)
                            .FirstOrDefault()
                    })
                    .FirstOrDefaultAsync();

                // Hold Queue
                var holdQueue = await _context.Tokens
                    .Include(x => x.Patient)
                    .Include(x => x.Status)
                    .Where(x => x.DoctorId == doctorId
                             && x.Status.Id == 4 && x.ServiceId == 2 && x.IsDeleted == false)
                    .Select(x => new QueuePatientDto
                    {
                        Id = x.Id,
                        TokenNumber = x.TokenNumber,
                        PatientName = x.Patient.Name,
                        PriorityName = _context.PriorityConfigs
                            .Where(p => p.Id == x.Id)
                            .Select(p => p.Name)
                            .FirstOrDefault()
                    })
                    .ToListAsync();

                // Skipped Queue
                var skippedQueue = await _context.Tokens
                    .Include(x => x.Patient)
                    .Include(x => x.Status)
                    .Where(x => x.DoctorId == doctorId
                             && x.Status.Id == 5 && x.ServiceId == 2 && x.IsDeleted == false)
                    .Select(x => new QueuePatientDto
                    {
                        Id = x.Id,
                        TokenNumber = x.TokenNumber,
                        PatientName = x.Patient.Name,
                        PriorityName = _context.PriorityConfigs
                            .Where(p => p.Id == x.Id)
                            .Select(p => p.Name)
                            .FirstOrDefault()
                    }).ToListAsync();

                var WaitingforReview = await _context.Tokens
                    .Include(x => x.Patient)
                    .Include(x => x.Status)
                    .Where(x => x.DoctorId == doctorId
                             && x.Status.Id == 11 && x.ServiceId == 2 && x.IsDeleted == false)
                    .Select(x => new QueuePatientDto
                    {
                        Id = x.Id,
                        TokenNumber = x.TokenNumber,
                        PatientName = x.Patient.Name,
                        PriorityName = _context.PriorityConfigs
                            .Where(p => p.Id == x.Id)
                            .Select(p => p.Name)
                            .FirstOrDefault()
                    }).ToListAsync();



                // Served Count
                var servedCount = await _context.Tokens.Include(x => x.Status).CountAsync(x => x.DoctorId == doctorId && x.Status.Id == 3 && x.ServiceId == 2 && x.IsDeleted == false);
                var calledCount = await _context.Tokens.Include(x => x.Status).CountAsync(x => x.DoctorId == doctorId && x.Status.Id == 2 && x.ServiceId == 2 && x.IsDeleted == false);
                var completedCount = await _context.Tokens.Include(x => x.Status).CountAsync(x => x.DoctorId == doctorId && x.StatusId == 6 && x.ServiceId == 2 && x.IsDeleted == false);

                return new DoctorQueueResponseDto
                {
                    DoctorName = doctor.Name,
                    TerminalName = doctor.RoomNumber ?? "Room 101",
                    calledCount = calledCount,
                    TotalCount = waitingQueue.Count + skippedQueue.Count + holdQueue.Count + servedCount + calledCount + completedCount + WaitingforReview.Count,

                    //TotalCount= _context.Tokens.Where(x => x.DoctorId == doctorId && x.ServiceId==2 && x.IsDeleted == false).Count(),

                    ServedCount = servedCount,
                    CompletedCount = completedCount,
                    WaitingCount = waitingQueue.Count,
                    MissedCount = skippedQueue.Count,

                    //sentToLabCount= _context.Tokens.Include(x => x.Status).Where(x => x.DoctorId == doctorId && x.Status.Id==3 && x.Service.Id==2 && x.IsDeleted == false).Count(),
                    //sentToScanCount= _context.Tokens.Include(x => x.Status).Where(x => x.DoctorId == doctorId && x.Status.Id==4 && x.Service.Id==2 && x.IsDeleted == false).Count(),
                    //sentToBillingCount= _context.Tokens.Include(x => x.Status).Where(x => x.DoctorId == doctorId && x.Status.Id==5 && x.Service.Id==2 && x.IsDeleted == false).Count(),
                    //paidCount= _context.Tokens.Include(x => x.Status).Where(x => x.DoctorId == doctorId && x.Status.Id==6 && x.Service.Id==2 && x.IsDeleted == false).Count(),
                    //sentToPharmacyCount= _context.Tokens.Include(x => x.Status).Where(x => x.DoctorId == doctorId && x.Status.Id==7 && x.Service.Id==2 && x.IsDeleted == false).Count(),
                    //MedicineDispensed= _context.Tokens.Include(x => x.Status).Where(x => x.DoctorId == doctorId && x.Status.Id == 8 && x.Service.Id==2 && x.IsDeleted == false).Count(),
                    //SendToAdmissionCount= _context.Tokens.Include(x => x.Status).Where(x => x.DoctorId == doctorId && x.Status.Id == 9 && x.Service.Id==2 && x.IsDeleted == false).Count(),
                    //AdmittedCount= _context.Tokens.Include(x => x.Status).Where(x => x.DoctorId == doctorId && x.Status.Id == 10 && x.Service.Id==2 && x.IsDeleted == false).Count(),

                    CurrentTicket = currentTicket,

                    NextQueue = waitingQueue.Take(3).ToList(),

                    WaitingQueue = waitingQueue,
                    SkippedQueue = skippedQueue,
                    HoldQueue = holdQueue,
                    waitingForReviewQueue = WaitingforReview
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ApiResponse<DoctorQueueResponseDto>> UpdateQueueStatusAsync(UpdateQueueRequest request)
        {
            try
            {
                if (request == null)
                {
                    return new ApiResponse<DoctorQueueResponseDto>
                    {
                        Success = false,
                        Message = "Invalid request"
                    };
                }

                // Get Status IDs
                var WaitingStatusId = await _context.TokenStatuses.Where(x => x.Id == 1).Select(x => x.Id).FirstOrDefaultAsync();

                var calledStatusId = await _context.TokenStatuses.Where(x => x.Id == 2).Select(x => x.Id).FirstOrDefaultAsync();

                var completedStatusId = await _context.TokenStatuses.Where(x => x.Id == 6).Select(x => x.Id).FirstOrDefaultAsync();

                var holdStatusId = await _context.TokenStatuses.Where(x => x.Id == 4).Select(x => x.Id).FirstOrDefaultAsync();

                var skippedStatusId = await _context.TokenStatuses.Where(x => x.Id == 5).Select(x => x.Id).FirstOrDefaultAsync();

                var WaitingForReviewStatusId = await _context.TokenStatuses.Where(x => x.Id == 11).Select(x => x.Id).FirstOrDefaultAsync();

                var ServiceId = 2;

                switch (request?.Action?.ToUpper())
                {
                    /* ==========================================
                       CALL
                    ========================================== */
                    case "CALL":

                        // Check current called token
                        var activeToken = await _context.Tokens.FirstOrDefaultAsync(x => x.DoctorId == request.DoctorId && x.StatusId == calledStatusId && x.IsDeleted == false);

                        if (activeToken != null)
                        {
                            return new ApiResponse<DoctorQueueResponseDto>
                            {
                                Success = false,
                                Message = "Current patient still in consultation"
                            };
                        }

                        // Get first waiting token
                        var waitingToken = await _context.Tokens.Where(x => x.DoctorId == request.DoctorId && x.StatusId == WaitingStatusId && x.IsDeleted == false).OrderBy(x => x.CreatedAt).FirstOrDefaultAsync();

                        if (waitingToken == null)
                        {
                            return new ApiResponse<DoctorQueueResponseDto>
                            {
                                Success = false,
                                Message = "No waiting patient"
                            };
                        }

                        waitingToken.StatusId = calledStatusId;
                        waitingToken.ServiceId = ServiceId;
                        waitingToken.CalledAt = DateTime.Now;

                        break;

                    case "WAITINGCALL":

                        // Check current called token
                        var CalledToken = await _context.Tokens.FirstOrDefaultAsync(x => x.DoctorId == request.DoctorId && x.StatusId == calledStatusId && x.IsDeleted == false);

                        if (CalledToken != null)
                        {
                            return new ApiResponse<DoctorQueueResponseDto>
                            {
                                Success = false,
                                Message = "Current patient still in consultation"
                            };
                        }

                        // Get first waiting token
                        var waitingForDoctorReviewToken = await _context.Tokens.Where(x => x.DoctorId == request.DoctorId && x.StatusId == WaitingForReviewStatusId && x.IsDeleted == false).OrderBy(x => x.CreatedAt).FirstOrDefaultAsync();

                        if (waitingForDoctorReviewToken == null)
                        {
                            return new ApiResponse<DoctorQueueResponseDto>
                            {
                                Success = false,
                                Message = "No waiting patient"
                            };
                        }

                        waitingForDoctorReviewToken.StatusId = calledStatusId;
                        waitingForDoctorReviewToken.ServiceId = ServiceId;
                        waitingForDoctorReviewToken.CalledAt = DateTime.Now;

                        break;

                    /* ==========================================
                       END
                    ========================================== */
                    case "END":

                        var endToken = await _context.Tokens.FirstOrDefaultAsync(x => x.Id == request.TokenId && x.DoctorId == request.DoctorId && x.IsDeleted == false);

                        if (endToken == null)
                        {
                            return new ApiResponse<DoctorQueueResponseDto>
                            {
                                Success = false,
                                Message = "Token not found"
                            };
                        }

                        endToken.StatusId = completedStatusId;
                        endToken.ServiceId = ServiceId;
                        endToken.CompletedAt = DateTime.Now;

                        break;

                    /* ==========================================
                       HOLD
                    ========================================== */
                    case "HOLD":

                        var holdToken = await _context.Tokens.FirstOrDefaultAsync(x => x.Id == request.TokenId && x.DoctorId == request.DoctorId && x.IsDeleted == false);

                        if (holdToken == null)
                        {
                            return new ApiResponse<DoctorQueueResponseDto>
                            {
                                Success = false,
                                Message = "Token not found"
                            };
                        }

                        holdToken.StatusId = holdStatusId;
                        holdToken.ServiceId = ServiceId;

                        break;

                    /* ==========================================
                       SKIP
                    ========================================== */
                    case "SKIP":

                        var skipToken = await _context.Tokens.FirstOrDefaultAsync(x => x.Id == request.TokenId && x.DoctorId == request.DoctorId && x.IsDeleted == false);

                        if (skipToken == null)
                        {
                            return new ApiResponse<DoctorQueueResponseDto>
                            {
                                Success = false,
                                Message = "Token not found"
                            };
                        }

                        skipToken.StatusId = skippedStatusId;
                        skipToken.ServiceId = ServiceId;

                        break;

                    /* ==========================================
                       RECALL
                    ========================================== */
                    case "RECALL":

                        var recallToken = await _context.Tokens.FirstOrDefaultAsync(x => x.Id == request.TokenId && x.DoctorId == request.DoctorId && x.IsDeleted == false);

                        if (recallToken == null)
                        {
                            return new ApiResponse<DoctorQueueResponseDto>
                            {
                                Success = false,
                                Message = "Token not found"
                            };
                        }

                        recallToken.StatusId = calledStatusId;
                        recallToken.ServiceId = ServiceId;

                        recallToken.CalledAt = DateTime.Now;

                        break;

                    /* ==========================================
                       NEXT
                    ========================================== */
                    case "NEXT":

                        // Complete current token
                        var currentToken = await _context.Tokens.FirstOrDefaultAsync(x => x.Id == request.TokenId && x.DoctorId == request.DoctorId && x.IsDeleted == false);

                        if (currentToken != null)
                        {
                            currentToken.StatusId = completedStatusId;
                            currentToken.ServiceId = ServiceId;
                            currentToken.CompletedAt = DateTime.Now;
                        }

                        // Call next waiting token
                        var nextToken = await _context.Tokens.Where(x => x.DoctorId == request.DoctorId && x.StatusId == WaitingStatusId && x.IsDeleted == false)
                            .OrderBy(x => x.CreatedAt).FirstOrDefaultAsync();

                        // No more waiting patients
                        if (nextToken == null)
                        {
                            await _context.SaveChangesAsync();

                            var updatedQueues = await GetDoctorQueueAsync(request.DoctorId);

                            return new ApiResponse<DoctorQueueResponseDto>
                            {
                                Success = true,
                                Message = "No more waiting patient",
                                Data = updatedQueues
                            };
                        }

                        nextToken.StatusId = calledStatusId;
                        nextToken.ServiceId = ServiceId;
                        nextToken.CalledAt = DateTime.Now;


                        break;

                    default:

                        return new ApiResponse<DoctorQueueResponseDto>
                        {
                            Success = false,
                            Message = "Invalid action"
                        };
                }

                await _context.SaveChangesAsync();

                // Refresh dashboard
                var updatedQueue = await GetDoctorQueueAsync(request.DoctorId);

                return new ApiResponse<DoctorQueueResponseDto>
                {
                    Success = true,
                    Message = $"{request.Action} successful",
                    Data = updatedQueue
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<DoctorQueueResponseDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<ApiResponse<bool>> SaveConsultationAsync(SaveConsultationRequest request)
        {
            try
            {
                // ==========================
                // Save Consultation
                // ==========================

                var consultation = new PatientConsultation
                {
                    TokenId = request.TokenId,

                    DoctorId = request.DoctorId,

                    Diagnosis = request.Diagnosis,

                    PrescriptionNotes = request.PrescriptionNotes,

                    CreatedAt = DateTime.Now
                };

                await _context.PatientConsultations.AddAsync(consultation);

                await _context.SaveChangesAsync();

                // ==========================
                // Save Medicines
                // ==========================

                if (request.Medicines != null && request.Medicines.Any())
                {
                    var medicineList = request.Medicines.Select(x =>

                                new PatientPrescriptionMedicine
                                {
                                    ConsultationId = consultation.Id,

                                    MedicineId = x.MedicineId,

                                    MedicineName = x.MedicineName,

                                    Quantity = x.Quantity,

                                    Dosage = x.Dosage,

                                    Timing = x.Timing,

                                    FoodInstruction = x.Food,

                                    Days = x.Days,

                                    IsDispensed = false,

                                    CreatedAt = DateTime.Now
                                })
                            .ToList();

                    await _context.PatientPrescriptionMedicines.AddRangeAsync(medicineList);

                    await _context.SaveChangesAsync();
                }

                return new ApiResponse<bool>
                {
                    Success = true,

                    Message = "Prescription saved successfully"
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
        public async Task<ApiResponse<bool>> SendToLabAsync(SendToLabRequest request)
        {
            try
            {
                var labRequest =
                    new PatientLabRequest
                    {
                        TokenId = request.TokenId,

                        DoctorId = request.DoctorId,

                        LabTestName = string.Join(
                            ", ",
                            request.LabTests
                        ),

                        Notes = request.Notes,

                        CreatedAt = DateTime.Now
                    };

                await _context
                    .PatientLabRequests
                    .AddAsync(
                        labRequest
                    );

                var token =
                    await _context
                        .Tokens
                        .FirstOrDefaultAsync(
                            x =>
                            x.Id ==
                            request.TokenId
                        );

                var labstatus = await _context.TokenStatuses.FirstOrDefaultAsync(x => x.Id == 1);

                if (labstatus == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Waiting status not found in system"
                    };
                }

                var ServiceId = await _context.Services.FirstOrDefaultAsync(x => x.Id == 3);

                if (ServiceId == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Service for Lab is not found in system"
                    };
                }

                if (token != null)
                {
                    token.StatusId = labstatus.Id; // SentToLab
                    token.ServiceId = ServiceId.Id;
                }

                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,

                    Message = "Patient sent to Lab successfully"
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
        public async Task<ApiResponse<bool>> SendToScanAsync(SendToScanRequest request)
        {
            try
            {
                // Validate Request
                if (request == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Invalid request data"
                    };
                }

                if (request.TokenId <= 0)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Invalid token id"
                    };
                }

                if (request.DoctorId <= 0)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Invalid doctor id"
                    };
                }

                if (request.ScanTypes == null
                    || !request.ScanTypes.Any())
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Please select at least one scan"
                    };
                }

                // Check Token
                var token =
                    await _context
                        .Tokens
                        .FirstOrDefaultAsync(
                            x =>
                                x.Id ==
                                request.TokenId
                                && x.IsDeleted == false
                        );

                if (token == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Token not found"
                    };
                }

                var Scanstatus = await _context.TokenStatuses.FirstOrDefaultAsync(x => x.Id == 1);

                if (Scanstatus == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Waiting status not found in system"
                    };
                }

                var ServiceId = await _context.Services.FirstOrDefaultAsync(x => x.Id == 4);

                if (ServiceId == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Service for Scanning is not found in system"
                    };
                }

                // Save Scan Request
                var scanRequest =
                    new PatientScanRequest
                    {
                        TokenId =
                            request.TokenId,

                        DoctorId =
                            request.DoctorId,

                        ScanType =
                            string.Join(
                                ", ",
                                request.ScanTypes
                            ),

                        Notes =
                            request.Notes,

                        IsCompleted =
                            false,

                        CreatedAt =
                            DateTime.Now
                    };

                await _context
                    .PatientScanRequests
                    .AddAsync(
                        scanRequest
                    );

                // Update Token Status
                token.StatusId = Scanstatus.Id; // SentToScan
                token.ServiceId = ServiceId.Id;

                await _context
                    .SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message =
                        "Patient sent to scan successfully"
                };
            }
            catch (DbUpdateException ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Database error: {ex.InnerException?.Message ?? ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message =
                        $"Error sending patient to scan: {ex.Message}"
                };
            }
        }
        public async Task<ApiResponse<bool>> SendToBillingAsync(SendToBillingRequest request)
        {
            try
            {
                // Validate Request
                if (request == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Invalid billing request"
                    };
                }

                // Check Token
                var token = await _context.Tokens.FirstOrDefaultAsync(x => x.Id == request.TokenId && x.IsDeleted == false);

                if (token == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Patient token not found"
                    };
                }

                // Calculate Total
                decimal total = (request.ConsultationFee ?? 0) + (request.LabFee ?? 0) + (request.ScanFee ?? 0) + (request.MedicineFee ?? 0);

                // Save Billing Request
                var billing = new PatientBillingRequest
                {
                    TokenId = request.TokenId,

                    DoctorId = request.DoctorId,

                    ConsultationFee = request.ConsultationFee,

                    LabFee = request.LabFee,

                    ScanFee = request.ScanFee,

                    MedicineFee = request.MedicineFee,

                    TotalAmount = total,

                    Notes = request.Notes,

                    IsPaid = false,

                    CreatedAt = DateTime.Now
                };

                await _context.PatientBillingRequests.AddAsync(billing);

                var BillingStatus = await _context.TokenStatuses.FirstOrDefaultAsync(x => x.Id == 7);

                if (BillingStatus == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Billing status not found in system"
                    };
                }
                var ServiceId = await _context.Services.FirstOrDefaultAsync(x => x.Id == 5);

                if (ServiceId == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Service for billing is not found in system"
                    };
                }

                // Update Token Status
                token.StatusId = BillingStatus.Id; // SentToBilling
                token.ServiceId = ServiceId.Id;

                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Patient sent to billing successfully"
                };
            }
            catch (DbUpdateException ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Database error: {ex.InnerException?.Message ?? ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Error sending patient to billing: {ex.Message}"
                };
            }
        }
        public async Task<ApiResponse<bool>> SendToAdmitAsync(SendToAdmitRequest request)
        {
            try
            {
                await _context.PatientAdmissions.AddAsync(new PatientAdmission
                {
                    TokenId = request.TokenId,

                    DoctorId = request.DoctorId,

                    AdmissionReason = request.AdmissionReason,

                    WardType = request.WardType,

                    CreatedAt = DateTime.Now
                });

                var token = await _context.Tokens.FirstOrDefaultAsync(x => x.Id == request.TokenId);

                if (token == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,

                        Message = "Token not found"
                    };
                }

                var AdmitStatus = await _context.TokenStatuses.FirstOrDefaultAsync(x => x.Id == 1);

                if (AdmitStatus == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Admit status not found in system"
                    };
                }

                var ServiceId = await _context.Services.FirstOrDefaultAsync(x => x.Id == 1);

                if (ServiceId == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Service for Admin is not found in system"
                    };
                }

                // Sent To Admit
                token.StatusId = AdmitStatus.Id;
                token.ServiceId = ServiceId.Id;

                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,

                    Message = "Patient sent to Admit successfully"
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
        public async Task<List<MedicineDto>> GetMedicinesAsync()
        {
            try
            {
                return await _context.Medicines.Where(x => x.IsActive == true).OrderBy(x => x.MedicineName)
                    .Select(x =>
                        new MedicineDto
                        {
                            Id =
                                x.Id,

                            MedicineName =
                                x.MedicineName
                                + " "
                                + x.Strength
                        })
                    .ToListAsync();
            }
            catch
            {
                return new List<MedicineDto>();
            }
        }
        public async Task<ApiResponse<int>> AddMedicineAsync(AddMedicineRequest request)
        {
            try
            {
                var medicine = new Medicine
                {
                    MedicineName = request.MedicineName,

                    Strength = request.Strength,

                    MedicineType = request.MedicineType,

                    Price = request.Price,

                    IsActive = true,

                    CreatedAt = DateTime.Now
                };

                await _context.Medicines.AddAsync(medicine);

                await _context.SaveChangesAsync();

                return new ApiResponse<int>
                {
                    Success = true,

                    Data = medicine.Id,

                    Message = "Medicine added successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<int>
                {
                    Success = false,

                    Message = ex.Message
                };
            }
        }
        public async Task<List<LabTestDto>> GetLabTestsAsync()
        {
            try
            {
                var result = await _context.LabTests.Where(x => x.IsActive)
                        .Select(

                            x => new LabTestDto
                            {
                                Id = x.Id,

                                CategoryId = x.CategoryId,

                                CategoryName = x.LabTestCategory.CategoryName,

                                TestName = x.TestName
                            })
                        .ToListAsync();

                return result;
            }
            catch
            {
                return new List<LabTestDto>();
            }
        }
        public async Task<List<ScanTestDto>> GetScanTestsAsync()
        {
            return await _context.ScanTests.Include(x => x.ScanCategory)

                .Select(
                    x => new ScanTestDto
                    {
                        Id = x.Id,

                        CategoryId = x.ScanCategoryId,

                        CategoryName = x.ScanCategory.CategoryName,

                        TestName = x.TestName
                    })

                .ToListAsync();
        }
    }
}
