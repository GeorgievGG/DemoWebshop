using DemoWebshopApi.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DemoWebshopApi.Data
{
    public class WebshopContext : IdentityDbContext<User, ApplicationRole, Guid>
    {
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderLine> OrderLines { get; set; }
        public virtual DbSet<ShoppingBasket> ShoppingBaskets { get; set; }
        public virtual DbSet<ShoppingBasketLine> ShoppingBasketLines { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("AppDb");
            optionsBuilder.UseSqlServer(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetupProductsConfiguration(modelBuilder);
            SetupOrdersConfiguration(modelBuilder);
            SetupOrderLinesConfiguration(modelBuilder);
            SetupShoppingBasketsConfiguration(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private static void SetupProductsConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(t => t.Id);
            modelBuilder.Entity<Product>().Property(t => t.Name).HasMaxLength(120).IsRequired();
            modelBuilder.Entity<Product>().Property(t => t.Model).HasMaxLength(120).IsRequired();
            modelBuilder.Entity<Product>().Property(t => t.AvailableQuantity).IsRequired();
            modelBuilder.Entity<Product>().Property(t => t.Price).IsRequired();
        }

        private static void SetupOrdersConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().HasKey(t => t.Id);
            modelBuilder.Entity<Order>().Property(t => t.OrderDate).IsRequired();
            modelBuilder.Entity<Order>().Property(t => t.Paid).IsRequired();
            modelBuilder.Entity<Order>().HasOne(t => t.Client).WithMany(u => u.Orders).HasForeignKey(t => t.ClientId).OnDelete(DeleteBehavior.Restrict);
        }

        private void SetupOrderLinesConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderLine>().HasKey(t => t.Id);
            modelBuilder.Entity<OrderLine>().Property(t => t.Quantity).IsRequired();
            modelBuilder.Entity<OrderLine>().Property(t => t.Price).IsRequired();
            modelBuilder.Entity<OrderLine>().HasOne(t => t.Order).WithMany(u => u.OrderLines).HasForeignKey(t => t.OrderId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<OrderLine>().HasOne(t => t.Product).WithMany().HasForeignKey(t => t.ProductId).OnDelete(DeleteBehavior.Restrict);
        }

        private void SetupShoppingBasketsConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShoppingBasket>().HasKey(t => t.Id);
            modelBuilder.Entity<ShoppingBasket>().Property(t => t.LastVisited).IsRequired();
            modelBuilder.Entity<ShoppingBasket>().HasOne(t => t.Client).WithOne(t => t.Basket).HasForeignKey<ShoppingBasket>(t => t.ClientId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
