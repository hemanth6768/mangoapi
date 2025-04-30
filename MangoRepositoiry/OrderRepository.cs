using MangoApi.AppContext;
using MangoApi.MangoModels;
using Microsoft.EntityFrameworkCore;

namespace MangoApi.MangoRepositoiry
{
    public class OrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.Include(o => o.Items).ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(string id)
        {
            return await _context.Orders.Include(o => o.Items)
                                        .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            order.Id = Guid.NewGuid().ToString();
            order.Date = DateTime.UtcNow;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> UpdateOrderAsync(string id, Order updatedOrder)
        {
            var existingOrder = await _context.Orders.Include(o => o.Items)
                                                     .FirstOrDefaultAsync(o => o.Id == id);
            if (existingOrder == null) return null;

            existingOrder.CustomerName = updatedOrder.CustomerName;
            existingOrder.Address = updatedOrder.Address;
            existingOrder.Phone = updatedOrder.Phone;
            existingOrder.Status = updatedOrder.Status;
            existingOrder.Amount = updatedOrder.Amount;

            _context.OrderItems.RemoveRange(existingOrder.Items);
            existingOrder.Items = updatedOrder.Items;

            await _context.SaveChangesAsync();
            return existingOrder;
        }

        public async Task UpdateOrderStatusAsync(string id, string status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteOrderAsync(string id)
        {
            var order = await _context.Orders.Include(o => o.Items)
                                             .FirstOrDefaultAsync(o => o.Id == id);
            if (order != null)
            {
                _context.OrderItems.RemoveRange(order.Items);
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerAsync(string customerId)
        {
            return await _context.Orders.Include(o => o.Items)
                                        .Where(o => o.CustomerName == customerId)
                                        .ToListAsync();
        }
    }
}
