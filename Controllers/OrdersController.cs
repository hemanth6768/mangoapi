using MangoApi.MangoModels;
using MangoApi.MangoService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MangoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<OrderResponse>> Checkout(OrderRequest orderRequest)
        {
            try
            {
                if (orderRequest.Items == null || !orderRequest.Items.Any())
                    return BadRequest("Order must contain at least one item");

                if (string.IsNullOrEmpty(orderRequest.CustomerInfo?.Name) ||
                    string.IsNullOrEmpty(orderRequest.CustomerInfo?.Phone) ||
                    string.IsNullOrEmpty(orderRequest.CustomerInfo?.Address))
                    return BadRequest("Customer information is incomplete");

                var response = await _orderService.ProcessOrderAsync(orderRequest);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Checkout failed");
                return StatusCode(500, "An error occurred while processing your order");
            }
        }

       /* [HttpGet]
        public async Task<ActionResult<List<Order>>> GetOrders()
        {
            var orders = await _orderService.GetOrdersAsync();
            return Ok(orders);
        }
*/

        [HttpGet]

        public async Task<ActionResult<List<Order>>> GetOrders1()
        {
            var orders = await _orderService.GetAllOrdersAsync1();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(string id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }


        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] UpdateOrderStatusDto dto)
        {
            var updated = await _orderService.UpdateOrderStatusAsync(id, dto.Status);
            if (!updated)
                return NotFound();

            return NoContent();
        }
    }
}
