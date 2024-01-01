using Domain.Models;
using FluentValidation;
using Infrastructure.Dtos;
using Infrastructure.Repositories.Interfaces;
using Service.Dtos;
using Service.Services.Interfaces;

namespace Service.Services
{
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

        public Task<Product> Create(CreateProductDto productToCreate)
        {
            var validationResult = _createProductDtoValidator.Validate(productToCreate);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var newProduct = new Product(productToCreate.Name, productToCreate.Description, productToCreate.AvailableQuantity, productToCreate.Price);

            return _productRepository.Create(newProduct);
        }

        public async Task<Product> Update(int id, UpdateProductDto productToUpdate)
        {
            var validationResult = _updateProductDtoValidator.Validate(productToUpdate);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var product = await _productRepository.GetById(id);

            if (product == null)
                throw new ArgumentException("Product not found");

            var updatingProduct = new Product()
            {
                Id = product.Id,
                Name = productToUpdate.Name ?? product.Name,
                Description = productToUpdate.Description ?? product.Description,
                AvailableQuantity = productToUpdate.AvailableQuantity ?? product.AvailableQuantity,
                Price = productToUpdate.Price ?? product.Price
            };

            return await _productRepository.Update(updatingProduct);
        }

        public Task Delete(int id)
        {
            if(_productRepository.GetById(id) == null)
                throw new ArgumentException("Product not found");

            return _productRepository.Delete(id);
        }

        public async Task<Product> GetById(int id)
        {
            var product = await _productRepository.GetById(id);

            if(product == null)
                throw new ArgumentException("Product not found");

            return product;
        }

        public async Task<List<Product>> FindAll(FindProductsDto options)
        {
            var validationResult = _findProductsDtoValidator.Validate(options);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            return await _productRepository.FindAll(options);
        }
    }
}
