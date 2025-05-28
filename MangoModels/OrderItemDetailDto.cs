namespace MangoApi.MangoModels
{
    public class OrderItemDetailDto
    {
        public string OrderId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerAppartmentName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
       
        public string ProductName { get; set; }
       
        public int Quantity { get; set; }
    }
}
