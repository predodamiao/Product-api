using Domain.Models;
using Infrastructure.Dtos;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IProductRepository
    {
        public Task<Product> Create(Product productToCreate);
        public Task<Product> Update(Product productToUpdate);
        public Task Delete(int id);
        public Task<Product> GetById(int id);
        public Task<IEnumerable<Product>> FindAll(FindProductsDto options);
    }
}
