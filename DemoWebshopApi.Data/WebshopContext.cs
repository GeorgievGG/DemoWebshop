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

            base.OnModelCreating(modelBuilder);
        }

        private static void SetupProductsConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(t => t.Id);
            modelBuilder.Entity<Product>().Property(t => t.Name).IsRequired();
            modelBuilder.Entity<Product>().Property(t => t.Model).IsRequired();
            modelBuilder.Entity<Product>().Property(t => t.AvailableQuantity).IsRequired();
            modelBuilder.Entity<Product>().Property(t => t.Price).IsRequired();
        }
    }
}
