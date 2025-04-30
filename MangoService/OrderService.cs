using MangoApi.MangoModels;
using MangoApi.MangoRepositoiry;

namespace MangoApi.MangoService
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return _orderRepository.GetAllOrdersAsync();
        }

        public Task<Order> GetOrderByIdAsync(string id)
        {
            return _orderRepository.GetOrderByIdAsync(id);
        }

        public Task<Order> CreateOrderAsync(Order order)
        {
            return _orderRepository.CreateOrderAsync(order);
        }

        public Task<Order> UpdateOrderAsync(string id, Order order)
        {
            return _orderRepository.UpdateOrderAsync(id, order);
        }

        public Task UpdateOrderStatusAsync(string id, string status)
        {
            return _orderRepository.UpdateOrderStatusAsync(id, status);
        }

        public Task DeleteOrderAsync(string id)
        {
            return _orderRepository.DeleteOrderAsync(id);
        }

        public Task<IEnumerable<Order>> GetOrdersByCustomerAsync(string customerId)
        {
            return _orderRepository.GetOrdersByCustomerAsync(customerId);
        }
    }
}

