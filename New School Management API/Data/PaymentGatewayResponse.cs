namespace New_School_Management_API.Data
{
    public class PaymentGatewayResponse
    {
        public string TransactionId { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }

        public string PaymentUrl { get; set; }
    }
}
