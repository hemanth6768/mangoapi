using MangoApi.MangoModels;

namespace MangoApi.MangoService
{
    public interface ICartService
    {

        Task<Cart> GetCartAsync(string userId);
        Task<Cart> AddToCartAsync(string userId, string productId, int quantity);
        Task<Cart> UpdateCartItemQuantityAsync(string userId, string productId, int quantity);
        Task<Cart> RemoveFromCartAsync(string userId, string productId);
        Task ClearCartAsync(string userId);
    }
}
