using Api.Dtos;
using Domain.Models;
using Infrastructure.Database.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service.Dtos;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Test.Integration.Api.Test.V1.Controllers
{
    public class ProductsControllerIntegrationTest
    {
        public ProductsControllerIntegrationTest()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
        }

        [Fact]
        public async Task Get_ValidParameters_ReturnsOkResultWithData()
        {
            // Arrange;
            using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1/products?pageNumber=1&pageSize=2&propertyToOrderBy=Id");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<Product>>(content);

            Assert.Equal(2, content.Length);
            Assert.Equal(1, result![0].Id);
            Assert.Equal(2, result![1].Id);

        }
    }
}
