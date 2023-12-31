using Domain.Models;
using Infrastructure.Database.Context.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Context
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProductMap());
        }
    }
}
