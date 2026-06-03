using System;
using System.Collections.Generic;

namespace SmartClinic.Application.DTOs
{
    /// <summary>
    /// Response DTO for completing a lab test
    /// </summary>
    public class CompleteLabTestResponse
    {
        /// <summary>
        /// Indicates if the lab test was completed successfully
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Timestamp when the lab test was completed
        /// </summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// Updated lab queue list after completion
        /// </summary>
        public List<LabQueueDto> UpdatedLabQueue { get; set; } = new List<LabQueueDto>();

        /// <summary>
        /// Count of remaining lab tests in queue
        /// </summary>
        public int RemainingLabTests { get; set; }

        /// <summary>
        /// Completion status message
        /// </summary>
        public string? Message { get; set; }
    }
}
