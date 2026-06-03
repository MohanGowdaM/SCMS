using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Application.DTOs
{
    public class DoctorQueueResponseDto
    {
        public string? DoctorName { get; set; }

        public string? TerminalName { get; set; }

        public int? TotalCount { get; set; }

        public int? ServedCount { get; set; }

        public int? WaitingCount { get; set; }

        public int? MissedCount { get; set; }    
        
        public int? calledCount { get; set; }

        public int? sentToLabCount { get; set; }
        public int? sentToScanCount { get; set; }
        public int? sentToBillingCount { get; set; }
        public int? paidCount { get; set; }
        public int? sentToPharmacyCount { get; set; }

        public int? MedicineDispensed { get; set; }

        public int? SendToAdmissionCount { get; set; }

        public int? AdmittedCount { get; set; }

        public int? CompletedCount { get; set; }

        public QueuePatientDto? CurrentTicket { get; set; }

        public List<QueuePatientDto>? NextQueue { get; set; }

        public List<QueuePatientDto>? WaitingQueue { get; set; }

        public List<QueuePatientDto>? SkippedQueue { get; set; }

        public List<QueuePatientDto>? HoldQueue { get; set; }

        public List<QueuePatientDto>? waitingForReviewQueue { get; set; }
        
    }


    public class QueuePatientDto
    {
        public int? Id { get; set; }

        public string? TokenNumber { get; set; }

        public string? PatientName { get; set; }

        public string? PriorityName { get; set; }
    }
}
