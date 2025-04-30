namespace MangoApi.MangoModels
{
    public class Order
    {
        public string Id { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public List<OrderItem> Items { get; set; }
    }
}
