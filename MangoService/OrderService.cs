using MangoApi.MangoModels;
using MangoApi.MangoRepositoiry;

namespace MangoApi.MangoService
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private static readonly Dictionary<string, OrderResponse> _orders = new Dictionary<string, OrderResponse>();

        private readonly ILogger<OrderService> _logger;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderResponse> ProcessOrderAsync(OrderRequest orderRequest)
        {
            var order = new Order
            {
                CustomerName = orderRequest.CustomerInfo.Name,
                CustomerEmail = orderRequest.CustomerInfo.Email,
                CustomerPhone = orderRequest.CustomerInfo.Phone,
                CustomerAddress = orderRequest.CustomerInfo.Address,
                TotalAmount = orderRequest.TotalAmount,
                Items = orderRequest.Items.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity
                }).ToList()
            };

            var savedOrder = await _orderRepository.CreateOrderAsync(order);

            return new OrderResponse
            {
                OrderId = savedOrder.Id,
                Status = savedOrder.Status,
                EstimatedDelivery = order.CreatedAt.AddHours(24),
                TotalAmount = savedOrder.TotalAmount,
                Message = "Your order has been confirmed. Payment will be collected upon delivery."
            };
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _orderRepository.GetAllOrdersAsync();
        }

        public async Task<Order> GetOrderByIdAsync(string id)
        {
            return await _orderRepository.GetOrderByIdAsync(id);
        }

        public async Task<bool> UpdateOrderStatusAsync(string id, string newStatus)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return false;

            await _orderRepository.UpdateStatusAsync(order, newStatus);
            return true;
        }
    }
}

