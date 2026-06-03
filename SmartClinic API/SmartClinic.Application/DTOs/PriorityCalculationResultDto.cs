using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Application.DTOs
{
    public class PriorityCalculationResultDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? PriorityLevel { get; set; }
        public int? MinWeight { get; set; }
        public int? MaxWeight { get; set; }
        public int? TotalSymptomWeight { get; set; }
        public List<string>? MatchedSymptoms { get; set; } = new List<string>();
    }
}
