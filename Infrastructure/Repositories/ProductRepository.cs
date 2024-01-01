using Domain.Models;
using Infrastructure.Database.Context;
using Infrastructure.Dtos;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Infrastructure.Repositories
{
    internal class ProductRepository : IProductRepository
    {
        ApplicationDbContext DbContext { get; set; }

        public ProductRepository(ApplicationDbContext context)
        {
            DbContext = context;
        }

        public async Task<Product> Create(Product productToCreate)
        {
            await DbContext.Products.AddAsync(productToCreate);
            await DbContext.SaveChangesAsync();

            return productToCreate;
        }

        public async Task Delete(int id)
        {
            var productToDelete = await DbContext.Products.FindAsync(id);

            if (productToDelete == null) return;

            DbContext.Products.Remove(productToDelete);
            await DbContext.SaveChangesAsync();
        }

        public async Task<List<Product>> FindAll(FindProductsDto options)
        {
            IQueryable<Product> query = DbContext.Products;

            if (!string.IsNullOrEmpty(options.NameToFind))
            {
                query = query.Where(p => EF.Functions.ILike(p.Name, $"%{options.NameToFind}%"));
            }

            if (!string.IsNullOrEmpty(options.PropertyToOrderBy))
            {
                query.OrderBy(options.PropertyToOrderBy);
            }

            query = query.Skip(options.Pagination.PageNumber * options.Pagination.PageSize).Take(options.Pagination.PageSize);

            return await query.ToListAsync();
        }

        public async Task<Product?> GetById(int id)
        {
            return await DbContext.Products.FindAsync(id);
        }

        public async Task<Product> Update(Product productToUpdate)
        {
            DbContext.Products.Update(productToUpdate);
            await DbContext.SaveChangesAsync();

            return productToUpdate;
        }
    }
}
