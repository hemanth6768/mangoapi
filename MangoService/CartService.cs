using MangoApi.MangoModels;
using MangoApi.MangoRepositoiry;


namespace MangoApi.MangoService
{
    public class CartService : ICartService
    {

        private readonly ICartRepository _cartRepository;
        private readonly IProductService _productService;

        public CartService(ICartRepository cartRepository, IProductService productService)
        {
            _cartRepository = cartRepository;
            _productService = productService;
        }

        public async Task<Cart> GetCartAsync(string userId)
        {
            var cart = await _cartRepository.GetCartAsync(userId);
            return cart ?? new Cart { UserId = userId };
        }

        public async Task<Cart> AddToCartAsync(string userId, string productId, int quantity)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found");
            }

            var cart = await GetCartAsync(userId);
            var existingItem = cart.Items.FirstOrDefault(item => item.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Quantity = quantity
                });
            }

            if (string.IsNullOrEmpty(cart.Id))
            {
                await _cartRepository.AddCartAsync(cart);
            }
            else
            {
                await _cartRepository.UpdateCartAsync(cart);
            }

            return cart;
        }

        public async Task<Cart> UpdateCartItemQuantityAsync(string userId, string productId, int quantity)
        {
            var cart = await GetCartAsync(userId);
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
            {
                throw new KeyNotFoundException($"Item with product ID {productId} not found in cart");
            }

            if (quantity <= 0)
            {
                return await RemoveFromCartAsync(userId, productId);
            }

            item.Quantity = quantity;
            await _cartRepository.UpdateCartAsync(cart);

            return cart;
        }

        public async Task<Cart> RemoveFromCartAsync(string userId, string productId)
        {
            var cart = await GetCartAsync(userId);
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                cart.Items.Remove(item);
                await _cartRepository.UpdateCartAsync(cart);
            }

            return cart;
        }

        public async Task ClearCartAsync(string userId)
        {
            var cart = await GetCartAsync(userId);
            cart.Items.Clear();
            await _cartRepository.UpdateCartAsync(cart);
        }
    }
}

