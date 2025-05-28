using MangoApi.AppContext;
using MangoApi.MangoModels;
using Microsoft.EntityFrameworkCore;

namespace MangoApi.MangoRepositoiry
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
           



            return await _context.Orders.Include(o => o.Items).ToListAsync();
        }

        public async Task<List<OrderItemDetailDto>> GetAllOrdersAsync1()
        {
            var result = await _context.Orders
                .Include(o => o.Items)
                .GroupBy(o => new
                {
                    o.Id,
                    o.CustomerName,
                    o.CustomerPhone,
                    o.CustomerAppartmentName,
                    o.CustomerAddress,
                    o.TotalAmount,
                    o.Status,
                    o.CreatedAt
                })
                .Select(g => new OrderItemDetailDto
                {
                    OrderId = g.Key.Id.ToString(),
                    CustomerName = g.Key.CustomerName,
                    CustomerPhone = g.Key.CustomerPhone,
                    CustomerAppartmentName = g.Key.CustomerAppartmentName,
                    CustomerAddress = g.Key.CustomerAddress,
                    TotalAmount = g.Key.TotalAmount,
                    Status = g.Key.Status,
                    CreatedAt = g.Key.CreatedAt,
                    ProductName = string.Join(", ", g.SelectMany(o => o.Items)
                                                     .Select(i => $"{i.ProductName} *{i.Quantity}")),
                    Quantity = g.SelectMany(o => o.Items).Sum(i => i.Quantity)
                })
                .ToListAsync();

            return result;
        }






        public async Task<Order> GetOrderByIdAsync(string id)
        {
            return await _context.Orders.Include(o => o.Items)
                                        .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order?> GetByIdAsync(string id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task UpdateStatusAsync(Order order, string newStatus)
        {
            order.Status = newStatus;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
