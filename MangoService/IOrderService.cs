using MangoApi.MangoModels;

namespace MangoApi.MangoService
{
    public interface IOrderService
    {
        Task<OrderResponse> ProcessOrderAsync(OrderRequest orderRequest);
        Task<List<Order>> GetOrdersAsync();
        Task<Order> GetOrderByIdAsync(string id);

        Task<bool> UpdateOrderStatusAsync(string id, string newStatus);
    }
}
