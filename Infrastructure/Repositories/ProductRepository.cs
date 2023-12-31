using Domain.Models;
using Infrastructure.Dtos;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories
{
    internal class ProductRepository : IProductRepository
    {
        public Task<Product> Create(Product productToCreate)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> FindAll(FindProductsDto options)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> Update(Product productToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
