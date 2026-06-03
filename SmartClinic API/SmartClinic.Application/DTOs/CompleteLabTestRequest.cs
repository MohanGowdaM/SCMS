using System;

namespace SmartClinic.Application.DTOs
{    
    public class CompleteLabTestRequest
    {      
        public int Id { get; set; }        
        public int TokenId { get; set; }       
        public string? CompletionNotes { get; set; }       
        public DateTime? CompletionDateTime { get; set; }
    }
}
