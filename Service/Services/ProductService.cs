using Domain.Models;
using FluentValidation;
using Infrastructure.Dtos;
using Infrastructure.Repositories.Interfaces;
using Service.Dtos;
using Service.Services.Interfaces;

namespace Service.Services
{
    /// <summary>
    /// Product Service
    /// </summary>
    /// <param name="createProductDtoValidator"></param>
    /// <param name="updateProductDtoValidator"></param>
    /// <param name="findProductsDtoValidator"></param>
    /// <param name="productRepository"></param>
    public class ProductService(
            IValidator<CreateProductDto> createProductDtoValidator,
            IValidator<UpdateProductDto> updateProductDtoValidator,
            IValidator<FindProductsDto> findProductsDtoValidator,
            IProductRepository productRepository
            ) : IProductService
    {
        private readonly IValidator<CreateProductDto> _createProductDtoValidator = createProductDtoValidator;
        private readonly IValidator<UpdateProductDto> _updateProductDtoValidator = updateProductDtoValidator;
        private readonly IValidator<FindProductsDto> _findProductsDtoValidator = findProductsDtoValidator;
        private readonly IProductRepository _productRepository = productRepository;

        /// <inheritdoc/>
        public Task<Product> Create(CreateProductDto productToCreate)
        {
            var validationResult = _createProductDtoValidator.Validate(productToCreate);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var newProduct = new Product(productToCreate.Name, productToCreate.Description, productToCreate.AvailableQuantity, productToCreate.Price);

            return _productRepository.Create(newProduct);
        }

        /// <inheritdoc/>
        public async Task<Product> Update(int id, UpdateProductDto productToUpdate)
        {
            var validationResult = _updateProductDtoValidator.Validate(productToUpdate);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var product = await _productRepository.GetById(id);

            if (product == null)
                throw new ArgumentException("Product not found");

            product.Id = product.Id;
            product.Name = productToUpdate.Name ?? product.Name;
            product.Description = productToUpdate.Description ?? product.Description;
            product.AvailableQuantity = productToUpdate.AvailableQuantity ?? product.AvailableQuantity;
            product.Price = productToUpdate.Price ?? product.Price;

            return await _productRepository.Update(product);
        }

        /// <inheritdoc/>
        public async Task Delete(int id)
        {
            if(await _productRepository.GetById(id) == null)
                throw new ArgumentException("Product not found");

            await _productRepository.Delete(id);
        }

        /// <inheritdoc/>
        public async Task<Product> GetById(int id)
        {
            var product = await _productRepository.GetById(id);

            if(product == null)
                throw new ArgumentException("Product not found");

            return product;
        }

        /// <inheritdoc/>
        public async Task<List<Product>> FindAll(FindProductsDto options)
        {
            var validationResult = _findProductsDtoValidator.Validate(options);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            options.PropertyToOrderBy ??= nameof(Product.Id);

            return await _productRepository.FindAll(options);
        }
    }
}
