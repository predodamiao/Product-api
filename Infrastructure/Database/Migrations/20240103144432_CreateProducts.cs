using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class CreateProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "AvailableQuantity", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 100, "Powerful laptop with high-performance processor", "Laptop", 1299.99m },
                    { 2, 200, "Latest smartphone with advanced camera features", "Smartphone", 899.99m },
                    { 3, 50, "Noise-canceling headphones for immersive audio experience", "Headphones", 149.99m },
                    { 4, 30, "Fitness tracker and smartwatch with health monitoring", "Smartwatch", 199.99m },
                    { 5, 20, "Professional-grade camera for photography enthusiasts", "Camera", 1499.99m },
                    { 6, 80, "Bluetooth speaker with premium sound quality", "Wireless Speaker", 79.99m },
                    { 7, 10, "Next-gen gaming console for immersive gaming experience", "Gaming Console", 499.99m },
                    { 8, 15, "Ultra HD Smart TV with stunning picture quality", "4K TV", 799.99m },
                    { 9, 5, "High-performance drone with 4K camera for aerial photography", "Drone", 999.99m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 9);
        }
    }
}
