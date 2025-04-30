using MangoApi.MangoModels;

namespace MangoApi.MangoRepositoiry
{
    public interface ICartRepository
    {
        Task<Cart> GetCartAsync(string userId);
        Task AddCartAsync(Cart cart);
        Task UpdateCartAsync(Cart cart);
        Task SaveAsync();
    }
}
