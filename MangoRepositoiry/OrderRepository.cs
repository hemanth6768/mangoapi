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
            var orderWithItems = await (from o in _context.Orders
                                        join oi in _context.OrderItems
                                        on o.Id equals oi.OrderId
                                        select new OrderItemDetailDto
                                        {
                                            OrderId = o.Id,
                                            CustomerName = o.CustomerName,
                                            CustomerEmail = o.CustomerEmail,
                                            CustomerPhone = o.CustomerPhone,
                                            CustomerAddress = o.CustomerAddress,
                                            CustomerAppartmentName = o.CustomerAppartmentName,
                                            TotalAmount = o.TotalAmount,
                                            Status = o.Status,
                                            CreatedAt = o.CreatedAt,
                                            ProductName = oi.ProductName,
                                            Quantity = oi.Quantity
                                        }).ToListAsync();


            return orderWithItems;

            //return await _context.Orders.Include(o => o.Items).ToListAsync();
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
