using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Context.Mappings
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasColumnType("VARCHAR")
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasColumnName("Description")
                .HasColumnType("VARCHAR")
                .HasMaxLength(500);

            builder.Property(x => x.AvailableQuantity)
                .IsRequired()
                .HasColumnName("AvailableQuantity")
                .HasColumnType("INTEGER");

            builder.Property(x => x.Price)
                .IsRequired()
                .HasColumnName("Price")
                .HasColumnType("DECIMAL");
        }
    }
}
