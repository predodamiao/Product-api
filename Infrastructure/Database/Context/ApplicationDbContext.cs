using Domain.Models;
using Infrastructure.Database.Context.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Context
{
    /// <summary>
    /// DbContext of application
    /// </summary>
    /// <param name="options"></param>
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        /// <summary>  Products table </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>  Configure the model behavior on creating </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product(1, "Laptop", "Powerful laptop with high-performance processor", 100, (decimal)1299.99),
                new Product(2, "Smartphone", "Latest smartphone with advanced camera features", 200, (decimal)899.99),
                new Product(3, "Headphones", "Noise-canceling headphones for immersive audio experience", 50, (decimal)149.99),
                new Product(4, "Smartwatch", "Fitness tracker and smartwatch with health monitoring", 30, (decimal)199.99),
                new Product(5, "Camera", "Professional-grade camera for photography enthusiasts", 20, (decimal)1499.99),
                new Product(6, "Wireless Speaker", "Bluetooth speaker with premium sound quality", 80, (decimal)79.99),
                new Product(7, "Gaming Console", "Next-gen gaming console for immersive gaming experience", 10, (decimal)499.99),
                new Product(8, "4K TV", "Ultra HD Smart TV with stunning picture quality", 15, (decimal)799.99),
                new Product(9, "Drone", "High-performance drone with 4K camera for aerial photography", 5, (decimal)999.99)
            );

            modelBuilder.ApplyConfiguration(new ProductMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
