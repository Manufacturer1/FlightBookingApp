namespace BaseEntity.Responses
{
    public class PaymentResponse
    {
        public bool Success { get; set; }
        public string? ClientSecret { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
