namespace MangoApi.MangoModels
{
    public class OrderResponse
    {
        public string OrderId { get; set; }
        public string Status { get; set; }
        public DateTime EstimatedDelivery { get; set; }
        public decimal TotalAmount { get; set; }
        public string Message { get; set; }
    }
}
