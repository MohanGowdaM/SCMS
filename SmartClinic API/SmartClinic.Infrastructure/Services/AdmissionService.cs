using Microsoft.EntityFrameworkCore;
using SmartClinic.Application.DTOs;
using SmartClinic.Application.Interfaces;
using SmartClinic.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Infrastructure.Services
{
    public class AdmissionService : IAdmissionService
    {
        private readonly ApplicationDbContext _context;

        public AdmissionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AdmitQueueDto>>GetAdmitQueueAsync()
        {
            try
            {
                return await _context.PatientAdmissions.Where(x => x.IsAdmitted == false)
                    .Select(x =>
                        new AdmitQueueDto
                        {
                            Id = x.Id,

                            TokenId = x.TokenId,

                            TokenNumber = x.Token.TokenNumber,

                            PatientName =x.Token.Patient.Name,

                            WardType = x.WardType,

                            AdmissionReason = x.AdmissionReason
                        })
                    .ToListAsync();
            }
            catch
            {
                return new List<AdmitQueueDto>();
            }
        }

        public async Task<ApiResponse<bool>>CompleteAdmissionAsync(CompleteAdmissionRequest request)
        {
            try
            {
                var admission = await _context.PatientAdmissions.FirstOrDefaultAsync( x => x.Id == request.Id);

                if (admission == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Admission not found"
                    };
                }

                admission.BedNumber = request.BedNumber;

                admission.IsAdmitted = true;

                admission.AdmittedAt = DateTime.Now;

                var AdmitedStatus = await _context.TokenStatuses.FirstOrDefaultAsync(x => x.Name.ToLower() == "Admitted");

                if (AdmitedStatus == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Admited status not found in system"
                    };
                }


                var token = await _context.Tokens.FirstOrDefaultAsync(x => x.Id == request.TokenId);

                // Admitted
                token.StatusId = AdmitedStatus.Id;

                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Patient admitted successfully"
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
    }
}
