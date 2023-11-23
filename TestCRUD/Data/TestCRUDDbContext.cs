using Microsoft.EntityFrameworkCore;
using TestCRUD.Models.Domain;

namespace TestCRUD.Data
{
    public class TestCRUDDbContext : DbContext
    {

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public TestCRUDDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
            .HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<Provider>()
                .HasMany(p => p.Orders)
                .WithOne(o => o.Provider)
                .HasForeignKey(o => o.ProviderId);
        }
    }
}
