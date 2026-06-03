using Microsoft.EntityFrameworkCore;
using SmartClinic.Application.DTOs;
using SmartClinic.Application.Interfaces;
using SmartClinic.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Infrastructure.Services
{
    public class PharmacyService : IPharmacyService
    {
        private readonly ApplicationDbContext _context;

        public PharmacyService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<PharmacyQueueDto>>GetPharmacyQueueAsync()
        {
            try
            {
                return await _context.Tokens.Where(x => x.StatusId == 14)
                    .Select(x =>
                        new PharmacyQueueDto
                        {
                            Id =
                                x.Id,

                            TokenId = x.Id,

                            TokenNumber = x.TokenNumber,

                            PatientName = _context.Patients.Where(p => p.Id == x.PatientId).Select(p => p.Name).FirstOrDefault(),

                            //                        PrescriptionNotes =
                            //string.Join(

                            //    ", ",

                            //    _context
                            //        .PatientConsultations
                            //        .Where(p =>
                            //            p.TokenId ==
                            //            x.Id)
                            //        .Select(p =>
                            //            p.PrescriptionNotes)
                            //        .ToList()
                            //)

                            PrescriptionNotes = _context.PatientConsultations.Where(p => p.TokenId == x.Id).OrderByDescending(p => p.CreatedAt)
                                                .Select(p => p.PrescriptionNotes).FirstOrDefault()
                        }).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>>DispenseMedicineAsync(DispenseMedicineRequest request)
        {
            try
            {
                var token = await _context.Tokens.FirstOrDefaultAsync(x => x.Id == request.TokenId);

                if (token == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Patient token not found"
                    };
                }

                var billing = await _context.PatientBillingRequests.FirstOrDefaultAsync( x => x.TokenId == request.TokenId);

                if (billing != null)
                {
                    billing.MedicineFee = request.MedicineFee;

                    billing.TotalAmount = billing.ConsultationFee + billing.LabFee + billing.ScanFee + request.MedicineFee;
                }

                var MedicineDispense = await _context.TokenStatuses.FirstOrDefaultAsync(x => x.Name.ToLower() == "MedicineDispensed");

                if (MedicineDispense == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "MedicineDispense status not found in system"
                    };
                }

                token.StatusId = MedicineDispense.Id;

                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Medicine dispensed successfully"
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
