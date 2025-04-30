using MangoApi.MangoModels;

namespace MangoApi.MangoRepositoiry
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(string id);
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> UpdateOrderAsync(string id, Order order);
        Task UpdateOrderStatusAsync(string id, string status);
        Task DeleteOrderAsync(string id);
        Task<IEnumerable<Order>> GetOrdersByCustomerAsync(string customerId);
    }
}
