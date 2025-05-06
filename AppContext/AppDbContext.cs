using MangoApi.MangoModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MangoApi.AppContext
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optional: Customize entity configurations


            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(p => p.UserId);

            modelBuilder.Entity<IdentityUserRole<string>>()
           .HasKey(p => new { p.UserId, p.RoleId });


            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(p => p.UserId);
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.Items)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cart>()
            .HasKey(c => c.GuidId);

            modelBuilder.Entity<Cart>()
            .Property(c => c.GuidId)
            .IsRequired();

            modelBuilder.Entity<Cart>()
            .Property(c => c.GuidId)
            .HasDefaultValueSql("NEWID()");


            modelBuilder.Entity<CartItem>()
           .HasKey(c => c.ProductId);
        }
    }
}
