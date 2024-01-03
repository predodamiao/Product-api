using Domain.Models;
using FluentResults;
using Infrastructure.Dtos;
using Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Service.Dtos;
using Service.Services;
using Service.Services.Interfaces;
using Service.Services.Validations;

namespace Test.Unit.Service.Test
{
    public class ProductServiceTests
    {
        private readonly CreateProductDtoValidator _createProductDtoValidator;
        private readonly UpdateProductDtoValidator _updateProductDtoValidator;
        private readonly FindProductsDtoValidator _findProductsDtoValidator;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<ILogger<ProductService>> _loggerMock;
        private readonly string CACHE_PREFIX = "Product_";

        public ProductServiceTests()
        {
            _createProductDtoValidator = new CreateProductDtoValidator();
            _updateProductDtoValidator = new UpdateProductDtoValidator();
            _findProductsDtoValidator = new FindProductsDtoValidator();
            _productRepositoryMock = new Mock<IProductRepository>();
            _cacheServiceMock = new Mock<ICacheService>();
            _loggerMock = new Mock<ILogger<ProductService>>();
        }

        #region Create
        [Fact]
        public async Task Create_ValidProduct_ReturnsSuccessResultAndCachesProduct()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var validDto = new CreateProductDto("ValidProduct", "Description", 10, (decimal)20.0);
            _productRepositoryMock.Setup(x => x.Create(It.IsAny<Product>())).ReturnsAsync(new Product(1, "ValidProduct", "Description", 10, (decimal)20.0));

            // Act
            var result = await productService.Create(validDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(validDto.Name, result.Value.Name);
            Assert.Equal(validDto.Description, result.Value.Description);
            Assert.Equal(validDto.AvailableQuantity, result.Value.AvailableQuantity);
            Assert.Equal(validDto.Price, result.Value.Price);
            _cacheServiceMock.Verify(x => x.Set(It.IsAny<string>(), It.IsAny<Product>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Create_ValidProductWithEdgeCases_ReturnsSuccessResultAndCachesProduct()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var validDto = new CreateProductDto(new string('a', 100), new string('b', 500), 0, (decimal)0.0 );
            _productRepositoryMock.Setup(x => x.Create(It.IsAny<Product>())).ReturnsAsync(new Product (1, new string('a', 100), new string('b', 500), 0, (decimal)0.0 ));

            // Act
            var result = await productService.Create(validDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(validDto.Name, result.Value.Name);
            Assert.Equal(validDto.Description, result.Value.Description);
            Assert.Equal(validDto.AvailableQuantity, result.Value.AvailableQuantity);
            Assert.Equal(validDto.Price, result.Value.Price);
            _cacheServiceMock.Verify(x => x.Set(It.IsAny<string>(), It.IsAny<Product>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Create_InvalidProductWithoutName_ReturnsErrorWithMessage()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var invalidDto = new CreateProductDto("", "Description", 10, (decimal)20.0);

            // Act
            var result = await productService.Create(invalidDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains($"Invalid field: {nameof(CreateProductDto.Name)}", result.Errors.First().Message);
            Assert.Contains($"{nameof(CreateProductDto.Name)} is required", result.Errors.First().Reasons.First().Message);
            _cacheServiceMock.Verify(x => x.Set(It.IsAny<string>(), It.IsAny<Product>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Create_InvalidProductWithNameBiggerThan100Chars_ReturnsErrorWithMessage()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var invalidDto = new CreateProductDto(new string('a', 101), "Description", 10, (decimal)20.0);

            // Act
            var result = await productService.Create(invalidDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains($"Invalid field: {nameof(CreateProductDto.Name)}", result.Errors.First().Message);
            Assert.Contains($"{nameof(CreateProductDto.Name)} must be less than or equal to 100 characters", result.Errors.First().Reasons.First().Message);
            _cacheServiceMock.Verify(x => x.Set(It.IsAny<string>(), It.IsAny<Product>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Create_InvalidProductWithoutDescription_ReturnsErrorWithMessage()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var invalidDto = new CreateProductDto("ValidProduct", "", 10, (decimal)20.0);

            // Act
            var result = await productService.Create(invalidDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains($"Invalid field: {nameof(CreateProductDto.Description)}", result.Errors.First().Message);
            Assert.Contains($"{nameof(CreateProductDto.Description)} is required", result.Errors.First().Reasons.First().Message);
            _cacheServiceMock.Verify(x => x.Set(It.IsAny<string>(), It.IsAny<Product>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Create_InvalidProductWithDescriptionBiggerThan500Chars_ReturnsErrorWithMessage()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var invalidDto = new CreateProductDto("ValidProduct", new string('a', 501), 10, (decimal)20.0);

            // Act
            var result = await productService.Create(invalidDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains($"Invalid field: {nameof(CreateProductDto.Description)}", result.Errors.First().Message);
            Assert.Contains($"{nameof(CreateProductDto.Description)} must be less than or equal to 500 characters", result.Errors.First().Reasons.First().Message);
            _cacheServiceMock.Verify(x => x.Set(It.IsAny<string>(), It.IsAny<Product>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Create_InvalidProductWithNegativeAvailableQuantity_ReturnsErrorWithMessage()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var invalidDto = new CreateProductDto("ValidProduct", "Description", -1, (decimal)20.0);

            // Act
            var result = await productService.Create(invalidDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains($"Invalid field: {nameof(CreateProductDto.AvailableQuantity)}", result.Errors.First().Message);
            Assert.Contains($"{nameof(CreateProductDto.AvailableQuantity)} must be greater than or equal to 0", result.Errors.First().Reasons.First().Message);
            _cacheServiceMock.Verify(x => x.Set(It.IsAny<string>(), It.IsAny<Product>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Create_InvalidProductWithNegativePrice_ReturnsErrorWithMessage()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var invalidDto = new CreateProductDto("ValidProduct", "Description", 10, -1);

            // Act
            var result = await productService.Create(invalidDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains($"Invalid field: {nameof(CreateProductDto.Price)}", result.Errors.First().Message);
            Assert.Contains($"{nameof(CreateProductDto.Price)} must be greater than or equal to 0", result.Errors.First().Reasons.First().Message);
            _cacheServiceMock.Verify(x => x.Set(It.IsAny<string>(), It.IsAny<Product>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Create_ExceptionDuringCreation_ReturnsFailureResultAndDoesNotCacheProduct()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var validDto = new CreateProductDto("ValidProduct", "Description", 10, (decimal)20.0);
            _productRepositoryMock.Setup(x => x.Create(It.IsAny<Product>())).ThrowsAsync(new Exception("Simulated exception during creation"));

            // Act
            var result = await productService.Create(validDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Simulated exception during creation", result.Errors.First().Message);
            _cacheServiceMock.Verify(x => x.Set(It.IsAny<string>(), It.IsAny<Product>(), It.IsAny<int>()), Times.Never);
        }

        #endregion Create

        #region Update
        [Fact]
        public async Task Update_ExistingProduct_ValidDto_ReturnsSuccessResultAndCachesUpdatedProduct()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var existingProduct = new Product(1, "ExistingProduct", "Description", 5, (decimal)10.0);
            _productRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(existingProduct);
            _productRepositoryMock.Setup(x => x.Update(It.IsAny<Product>())).ReturnsAsync((Product p) => p);
            var updateDto = new UpdateProductDto("UpdatedProduct", "UpdatedDescription", 10, (decimal)20.0);

            // Act
            var result = await productService.Update(existingProduct.Id, updateDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(updateDto.Name, result.Value.Name);
            Assert.Equal(updateDto.Description, result.Value.Description);
            Assert.Equal(updateDto.AvailableQuantity, result.Value.AvailableQuantity);
            Assert.Equal(updateDto.Price, result.Value.Price);
            _cacheServiceMock.Verify(x => x.Set(It.IsAny<string>(), It.IsAny<Product>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Update_NonExistingProduct_ReturnsFailureResultAndDoesNotCache()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            _productRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(default(Product));
            _productRepositoryMock.Setup(x => x.Update(It.IsAny<Product>())).ReturnsAsync((Product p) => p);
            var updateDto = new UpdateProductDto("UpdatedProduct", "UpdatedDescription", 10, (decimal)20.0);

            // Act
            var result = await productService.Update(1, updateDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Product not found", result.Errors.First().Message);
            _cacheServiceMock.Verify(x => x.Set(It.IsAny<string>(), It.IsAny<Product>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Update_JustName_ReturnsSuccessResultAndCachesUpdatedProduct()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var existingProduct = new Product(1, "ExistingProduct", "Description", 5, (decimal)10.0);
            _productRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(existingProduct);
            _productRepositoryMock.Setup(x => x.Update(It.IsAny<Product>())).ReturnsAsync((Product p) => p);
            var updateDto = new UpdateProductDto("UpdatedName", null, null, null);

            // Act
            var result = await productService.Update(existingProduct.Id, updateDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(updateDto.Name, result.Value.Name);
            Assert.Equal(existingProduct.Description, result.Value.Description);
            Assert.Equal(existingProduct.AvailableQuantity, result.Value.AvailableQuantity);
            Assert.Equal(existingProduct.Price, result.Value.Price);
            _cacheServiceMock.Verify(x => x.Set(It.IsAny<string>(), It.IsAny<Product>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Update_JustDescription_ReturnsSuccessResultAndCachesUpdatedProduct()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var existingProduct = new Product(1, "ExistingProduct", "Description", 5, (decimal)10.0);
            _productRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(existingProduct);
            _productRepositoryMock.Setup(x => x.Update(It.IsAny<Product>())).ReturnsAsync((Product p) => p);
            var updateDto = new UpdateProductDto(null, "UpdatedDescription", null, null);

            // Act
            var result = await productService.Update(existingProduct.Id, updateDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(existingProduct.Name, result.Value.Name);
            Assert.Equal(updateDto.Description, result.Value.Description);
            Assert.Equal(existingProduct.AvailableQuantity, result.Value.AvailableQuantity);
            Assert.Equal(existingProduct.Price, result.Value.Price);
            _cacheServiceMock.Verify(x => x.Set(It.IsAny<string>(), It.IsAny<Product>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Update_JustAvailableQuantity_ReturnsSuccessResultAndCachesUpdatedProduct()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var existingProduct = new Product(1, "ExistingProduct", "Description", 5, (decimal)10.0);
            _productRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(existingProduct);
            _productRepositoryMock.Setup(x => x.Update(It.IsAny<Product>())).ReturnsAsync((Product p) => p);
            var updateDto = new UpdateProductDto(null, null, 20, null);

            // Act
            var result = await productService.Update(existingProduct.Id, updateDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(existingProduct.Name, result.Value.Name);
            Assert.Equal(existingProduct.Description, result.Value.Description);
            Assert.Equal(updateDto.AvailableQuantity, result.Value.AvailableQuantity);
            Assert.Equal(existingProduct.Price, result.Value.Price);
            _cacheServiceMock.Verify(x => x.Set(It.IsAny<string>(), It.IsAny<Product>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Update_JustPrice_ReturnsSuccessResultAndCachesUpdatedProduct()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var existingProduct = new Product(1, "ExistingProduct", "Description", 5, (decimal)10.0);
            _productRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(existingProduct);
            _productRepositoryMock.Setup(x => x.Update(It.IsAny<Product>())).ReturnsAsync((Product p) => p);
            var updateDto = new UpdateProductDto(null, null, null, (decimal)15.0);

            // Act
            var result = await productService.Update(existingProduct.Id, updateDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(existingProduct.Name, result.Value.Name);
            Assert.Equal(existingProduct.Description, result.Value.Description);
            Assert.Equal(existingProduct.AvailableQuantity, result.Value.AvailableQuantity);
            Assert.Equal(updateDto.Price, result.Value.Price);
            _cacheServiceMock.Verify(x => x.Set(It.IsAny<string>(), It.IsAny<Product>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Update_InvalidProductWithNameBiggerThan100Chars_ReturnsErrorWithMessage()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var existingProduct = new Product(1, "ExistingProduct", "Description", 5, (decimal)10.0);
            _productRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(existingProduct);
            var updateDto = new UpdateProductDto(new string('a', 101), null, null, null);

            // Act
            var result = await productService.Update(existingProduct.Id, updateDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains($"Invalid field: {nameof(UpdateProductDto.Name)}", result.Errors.First().Message);
            Assert.Contains($"{nameof(UpdateProductDto.Name)} must be less than or equal to 100 characters", result.Errors.First().Reasons.First().Message);
            _cacheServiceMock.Verify(x => x.Set(It.IsAny<string>(), It.IsAny<Product>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Update_InvalidProductWithDescriptionBiggerThan500Chars_ReturnsErrorWithMessage()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var existingProduct = new Product(1, "ExistingProduct", "Description", 5, (decimal)10.0);
            _productRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(existingProduct);
            var updateDto = new UpdateProductDto(null, new string('b', 501), null, null);

            // Act
            var result = await productService.Update(existingProduct.Id, updateDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains($"Invalid field: {nameof(UpdateProductDto.Description)}", result.Errors.First().Message);
            Assert.Contains($"{nameof(UpdateProductDto.Description)} must be less than or equal to 500 characters", result.Errors.First().Reasons.First().Message);
            _cacheServiceMock.Verify(x => x.Set(It.IsAny<string>(), It.IsAny<Product>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Update_ExceptionDuringUpdating_ReturnsFailureResultAndDoesNotCacheProduct()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var existingProduct = new Product(1, "ExistingProduct", "Description", 5, (decimal)10.0);
            _productRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(existingProduct);
            _productRepositoryMock.Setup(x => x.Update(It.IsAny<Product>())).ThrowsAsync(new Exception("Simulated exception during update"));
            var updateDto = new UpdateProductDto("UpdatedProduct", "UpdatedDescription", 10, (decimal)20.0);

            // Act
            var result = await productService.Update(existingProduct.Id, updateDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Simulated exception during update", result.Errors.First().Message);
            _cacheServiceMock.Verify(x => x.Set(It.IsAny<string>(), It.IsAny<Product>(), It.IsAny<int>()), Times.Never);
        }
        #endregion Update

        #region Delete
        [Fact]
        public async Task Delete_ExistingProduct_ReturnsSuccessResultAndDeleteCachedProduct()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var existingProduct = new Product(1, "ExistingProduct", "Description", 5, (decimal)10.0);
            _productRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(existingProduct);
            _productRepositoryMock.Setup(x => x.Delete(It.IsAny<int>())).Returns(Task.CompletedTask);
            _cacheServiceMock.Setup(x => x.Remove(It.IsAny<string>())).Verifiable();

            // Act
            var result = await productService.Delete(existingProduct.Id);

            // Assert
            Assert.True(result.IsSuccess);
            _cacheServiceMock.Verify(x => x.Remove($"{CACHE_PREFIX}{existingProduct.Id}"), Times.Once);
        }

        [Fact]
        public async Task Delete_NonExistingProduct_ReturnsFailureResultAndChangeCache()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            _productRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(default(Product));
            _productRepositoryMock.Setup(x => x.Delete(It.IsAny<int>())).Returns(Task.CompletedTask);
            _cacheServiceMock.Setup(x => x.Remove(It.IsAny<string>())).Verifiable();

            // Act
            var result = await productService.Delete(1);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Product not found", result.Errors.First().Message);
            _cacheServiceMock.Verify(x => x.Remove(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Delete_ExceptionDuringDeletion_ReturnsFailureResultAndDoesNotCacheProduct()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var existingProduct = new Product(1, "ExistingProduct", "Description", 5, (decimal)10.0);
            _productRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(existingProduct);
            _productRepositoryMock.Setup(x => x.Delete(It.IsAny<int>())).ThrowsAsync(new Exception("Simulated exception during deletion"));
            _cacheServiceMock.Setup(x => x.Remove(It.IsAny<string>())).Verifiable();

            // Act
            var result = await productService.Delete(existingProduct.Id);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Simulated exception during deletion", result.Errors.First().Message);
            _cacheServiceMock.Verify(x => x.Remove(It.IsAny<string>()), Times.Never);
        }
        #endregion Delete

        #region GetById
        [Fact]
        public async Task GetById_ExistingProductJustInDatabase_ReturnsSuccessResultAndProductFromDatabaseAndSaveProductOnCache()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var existingProduct = new Product(1, "ExistingProduct", "Description", 5, (decimal)10.0);
            _productRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(existingProduct);
            _cacheServiceMock.Setup(x => x.Get<Product>(It.IsAny<string>())).Returns(default(Product));
            _cacheServiceMock.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<Product>(), It.IsAny<int>())).Verifiable();

            // Act
            var result = await productService.GetById(existingProduct.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(existingProduct, result.Value);
            _cacheServiceMock.Verify(x => x.Set($"{CACHE_PREFIX}{existingProduct.Id}", existingProduct, It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetById_ExistingProductInCache_ReturnsSuccessResultAndProductFromCache()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var existingProduct = new Product(1, "ExistingProduct", "Description", 5, (decimal)10.0);
            _productRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(default(Product));
            _cacheServiceMock.Setup(x => x.Get<Product>(It.IsAny<string>())).Returns(existingProduct);

            // Act
            var result = await productService.GetById(existingProduct.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(existingProduct, result.Value);
            _productRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Never); 
        }

        [Fact]
        public async Task GetById_NonExistingProduct_ReturnsFailureResultAndDoesNotCacheProduct()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            _productRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(default(Product));
            _cacheServiceMock.Setup(x => x.Get<Product>(It.IsAny<string>())).Returns(default(Product));

            // Act
            var result = await productService.GetById(1);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Product not found", result.Errors.First().Message);
            _cacheServiceMock.Verify(x => x.Set(It.IsAny<string>(), It.IsAny<Product>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetById_ExceptionDuringGettingFromDatabase_ReturnsFailureResultAndDoesNotCacheProduct()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            _productRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ThrowsAsync(new Exception("Simulated exception during getting from database"));
            _cacheServiceMock.Setup(x => x.Get<Product>(It.IsAny<string>())).Returns(default(Product));

            // Act
            var result = await productService.GetById(1);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Simulated exception during getting from database", result.Errors.First().Message);
            _cacheServiceMock.Verify(x => x.Set(It.IsAny<string>(), It.IsAny<Product>(), It.IsAny<int>()), Times.Never);
        }
        #endregion GetById

        #region FindAll
        [Fact]
        public async Task FindAll_MatchingProductJustInDatabase_ReturnsSuccessResultAndProductFromDatabase()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var matchingOptions = new FindProductsDto { NameToFind = "Product", Pagination = new PaginationDto(1, 10) };
            var productsFromDatabase = new List<Product> { new Product(1, "Product", "Description", 5, (decimal)10.0) };

            _productRepositoryMock.Setup(x => x.FindAll(It.IsAny<FindProductsDto>())).ReturnsAsync(productsFromDatabase);

            // Act
            var result = await productService.FindAll(matchingOptions);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(productsFromDatabase, result.Value);
        }

        [Fact]
        public async Task FindAll_NonExistingMatchingProduct_ReturnsSuccessResultAndEmptyList()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var nonExistingMatchingOptions = new FindProductsDto { NameToFind = "NonExistingProduct", Pagination = new PaginationDto(1, 10) };

            _productRepositoryMock.Setup(x => x.FindAll(It.IsAny<FindProductsDto>())).ReturnsAsync(new List<Product>());

            // Act
            var result = await productService.FindAll(nonExistingMatchingOptions);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Value);
        }

        [Fact]
        public async Task FindAll_InvalidNameToFindBiggetThan100Char_ReturnsFailureResult()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var invalidNameToFindOptions = new FindProductsDto { NameToFind = new string('a', 101) };

            // Act
            var result = await productService.FindAll(invalidNameToFindOptions);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains($"Invalid field: {nameof(FindProductsDto.NameToFind)}", result.Errors.First().Message);
            Assert.Contains($"{nameof(FindProductsDto.NameToFind)} must be less than or equal to 100 characters", result.Errors.First().Reasons.First().Message);
        }

        [Fact]
        public async Task FindAll_InvalidPropertyToOrderBy_ReturnsFailureResult()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var invalidPropertyToOrderByOptions = new FindProductsDto { PropertyToOrderBy = "InvalidProperty" };
            var productProperties = typeof(Product).GetProperties().Select(x => x.Name);
            // Act
            var result = await productService.FindAll(invalidPropertyToOrderByOptions);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains($"Invalid field: {nameof(FindProductsDto.PropertyToOrderBy)}", result.Errors.First().Message);
            Assert.Contains($"{nameof(FindProductsDto.PropertyToOrderBy)} should be one of those options: {string.Join(',', productProperties)}", result.Errors.First().Reasons.First().Message);
        }

        [Fact]
        public async Task FindAll_WithoutPagination_ReturnsFailureResult()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var optionsWithoutPagination = new FindProductsDto();
            optionsWithoutPagination.Pagination = default!;

            // Act
            var result = await productService.FindAll(optionsWithoutPagination);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains($"Invalid field: {nameof(FindProductsDto.Pagination)}", result.Errors.First().Message);
            Assert.Contains($"{nameof(FindProductsDto.Pagination)} is required", result.Errors.First().Reasons.First().Message);
        }

        [Fact]
        public async Task FindAll_InvalidaPageNumberLessThan1_ReturnsFailureResult()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var invalidPageNumberOptions = new FindProductsDto { Pagination = new PaginationDto (0, 10) };

            // Act
            var result = await productService.FindAll(invalidPageNumberOptions);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid field: Pagination.PageNumber", result.Errors.First().Message);
            Assert.Contains($"{nameof(FindProductsDto.Pagination.PageNumber)} must be greater than or equal to 1", result.Errors.First().Reasons.First().Message);
        }

        [Fact]
        public async Task FindAll_InvalidaPageSizeLessThan1_ReturnsFailureResult()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var invalidPageSizeOptions = new FindProductsDto { Pagination = new PaginationDto(1, 0) };

            // Act
            var result = await productService.FindAll(invalidPageSizeOptions);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid field: Pagination.PageSize", result.Errors.First().Message);
            Assert.Contains($"{nameof(FindProductsDto.Pagination.PageSize)} must be greater than or equal to 1", result.Errors.First().Reasons.First().Message);
        }

        [Fact]
        public async Task FindAll_InvalidaPageSizeMoreThan200_ReturnsFailureResult()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var invalidPageSizeOptions = new FindProductsDto { Pagination = new PaginationDto(1, 201) };

            // Act
            var result = await productService.FindAll(invalidPageSizeOptions);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid field: Pagination.PageSize", result.Errors.First().Message);
            Assert.Contains($"{nameof(FindProductsDto.Pagination.PageSize)} must be less than or equal to 200", result.Errors.First().Reasons.First().Message);
        }

        [Fact]
        public async Task FindAll_ExceptionDuringGettingFromDatabase_ReturnsFailureResult()
        {
            // Arrange
            var productService = new ProductService(_loggerMock.Object, _createProductDtoValidator, _updateProductDtoValidator, _findProductsDtoValidator, _productRepositoryMock.Object, _cacheServiceMock.Object);
            var validOptions = new FindProductsDto
            {
                NameToFind = "Product",
                PropertyToOrderBy = "Name",
                Pagination = new PaginationDto(1, 10)
            };

            _productRepositoryMock.Setup(x => x.FindAll(It.IsAny<FindProductsDto>())).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await productService.FindAll(validOptions);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Database error", result.Errors.First().Message);
        }

        #endregion FindAll
    }
}
