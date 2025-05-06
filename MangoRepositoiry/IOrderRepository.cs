using MangoApi.MangoModels;

namespace MangoApi.MangoRepositoiry
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(string id);

        Task<Order?> GetByIdAsync(string id);
        Task UpdateStatusAsync(Order order, string newStatus);
    }
}
