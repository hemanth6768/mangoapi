using MangoApi.MangoModels;
using MangoApi.MangoService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MangoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<Cart>> GetCart(string userId)
        {
            var cart = await _cartService.GetCartAsync(userId);
            if (cart == null)
            {
                return NotFound();
            }
            return Ok(cart);
        }

        [HttpPost("{userId}/items")]
        public async Task<ActionResult<Cart>> AddToCart(string userId, [FromBody] CartItemRequest request)
        {
            var updatedCart = await _cartService.AddToCartAsync(userId, request.ProductId, request.Quantity);
            return Ok(updatedCart);
        }

        [HttpPut("{userId}/items/{productId}")]
        public async Task<ActionResult<Cart>> UpdateCartItem(string userId, string productId, [FromBody] UpdateCartItemRequest request)
        {
            var updatedCart = await _cartService.UpdateCartItemQuantityAsync(userId, productId, request.Quantity);
            return Ok(updatedCart);
        }

        [HttpDelete("{userId}/items/{productId}")]
        public async Task<ActionResult<Cart>> RemoveFromCart(string userId, string productId)
        {
            var updatedCart = await _cartService.RemoveFromCartAsync(userId, productId);
            return Ok(updatedCart);
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult> ClearCart(string userId)
        {
            await _cartService.ClearCartAsync(userId);
            return NoContent();
        }
    }

    // Request models
    public class CartItemRequest
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateCartItemRequest
    {
        public int Quantity { get; set; }
    }
}

