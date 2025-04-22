namespace BaseEntity.PaymentDtos
{
    public class CreatePaymentDto
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "usd";
    }
}
