using Domain.Models;
using Infrastructure.Dtos;
using Service.Dtos;

namespace Service.Services.Interfaces
{
    /// <summary>
    /// Product Service
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Create a product
        /// </summary>
        /// <param name="productToCreate"></param>
        /// <returns></returns>
        public Task<Product> Create(CreateProductDto productToCreate);
        /// <summary>
        /// Update a product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productToUpdate"></param>
        /// <returns></returns>
        public Task<Product> Update(int id, UpdateProductDto productToUpdate);
        /// <summary>
        /// Delete a product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task Delete (int id);
        /// <summary>
        /// Get a product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Product> GetById(int id);
        /// <summary>
        /// Find all products with filters
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public Task<List<Product>> FindAll(FindProductsDto options);
    }
}
