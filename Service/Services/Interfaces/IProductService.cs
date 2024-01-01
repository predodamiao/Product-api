using Domain.Models;
using Infrastructure.Dtos;
using Service.Dtos;

namespace Service.Services.Interfaces
{
    public interface IProductService
    {
        public Task<Product> Create(CreateProductDto productToCreate);
        public Task<Product> Update(int id, UpdateProductDto productToUpdate);
        public Task Delete (int id);
        public Task<Product> GetById(int id);
        public Task<List<Product>> FindAll(FindProductsDto options);
    }
}
