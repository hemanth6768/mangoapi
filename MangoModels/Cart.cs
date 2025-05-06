namespace MangoApi.MangoModels
{
    public class Cart
    {
        public Guid GuidId { get; set; }

        public string UserId { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal TotalPrice => Items.Sum(item => item.Price * item.Quantity);
    }
}
