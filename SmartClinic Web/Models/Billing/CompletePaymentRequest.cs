namespace SmartClinic.Web.Models.Billing
{
    public class CompletePaymentRequest
    {
        public int BillingId { get; set; }

        public int TokenId { get; set; }

        public string? PaymentMethod { get; set; }

        public string? Remarks { get; set; }
    }
}
