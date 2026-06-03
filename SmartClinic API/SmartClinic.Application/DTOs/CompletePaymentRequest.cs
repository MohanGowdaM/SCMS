using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Application.DTOs
{
    public class CompletePaymentRequest
    {
        public int BillingId { get; set; }

        public int TokenId { get; set; }

        public string? PaymentMethod { get; set; }

        public string? Remarks { get; set; }
    }
}
