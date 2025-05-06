using MangoApi.AppContext;
using MangoApi.MangoModels;
using Microsoft.EntityFrameworkCore;

namespace MangoApi.MangoRepositoiry
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetCartAsync(string userId)
        {
            return await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task AddCartAsync(Cart cart)
        {
            // Check if the cart Id is not set, then generate a new GUID for it
            if (cart.GuidId == Guid.Empty)
            {
                cart.GuidId = Guid.NewGuid();
            }

            _context.Carts.Add(cart);  // Add the cart to the DbContext
            await _context.SaveChangesAsync();  // Save changes to the database
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

    
}

