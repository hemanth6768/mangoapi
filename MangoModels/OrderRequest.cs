namespace MangoApi.MangoModels
{
    public class OrderRequest
    {
        public CustomerInfo CustomerInfo { get; set; }
        public List<OrderItemRequest> Items { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
