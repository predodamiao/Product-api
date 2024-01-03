using System.Net;
using Api.Dtos;
using Api.V1.Controllers;
using Domain.Models;
using FluentResults;
using Infrastructure.Dtos;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Service.Dtos;
using Service.Services.Interfaces;

namespace Test.Unit.Api.Test.V1.Controllers
{
    public class ProductsControllerTest
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly ProductsController _productsController;

        public ProductsControllerTest()
        {
            _productServiceMock = new Mock<IProductService>();
            _productsController = new ProductsController(_productServiceMock.Object);
        }

        #region Get

        [Fact]
        public async Task Get_ValidParameters_ReturnsOkResultWithData()
        {
            // Arrange
            var expectedResult = new List<Product> { new Product(1, "Product1", "Description", 5, (decimal)10.0) };
            _productServiceMock.Setup(x => x.FindAll(It.IsAny<FindProductsDto>())).ReturnsAsync(Result.Ok(expectedResult));

            // Act
            var result = await _productsController.Get(1, 10, "Product1", "Name");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedResult, okResult.Value);
        }

        [Fact]
        public async Task Get_InvalidParameters_ReturnsBadRequestWithError()
        {
            // Arrange
            var expectedResult = Result.Fail("Invalid parameters");
            _productServiceMock.Setup(x => x.FindAll(It.IsAny<FindProductsDto>())).ReturnsAsync(expectedResult);

            // Act
            var result = await _productsController.Get(0, 10, "InvalidProduct", "Name");

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResponse = Assert.IsType<ErrorResponseDto>(badRequestResult.Value);
            Assert.Equal((int)HttpStatusCode.BadRequest, errorResponse.Status);
            Assert.Single(errorResponse.Errors);
            Assert.Equal("Invalid parameters", errorResponse.Errors.First().Message);
        }

        #endregion

        #region GetById

        [Fact]
        public async Task GetById_ValidId_ReturnsOkResultWithData()
        {
            // Arrange
            var expectedResult = new Product(1, "Product1", "Description", 5, (decimal)10.0);
            _productServiceMock.Setup(x => x.GetById(1)).ReturnsAsync(Result.Ok(expectedResult));

            // Act
            var result = await _productsController.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedResult, okResult.Value);
        }

        [Fact]
        public async Task GetById_InvalidId_ReturnsBadRequestWithError()
        {
            // Arrange
            var expectedResult = Result.Fail("Product not found");
            _productServiceMock.Setup(x => x.GetById(0)).ReturnsAsync(expectedResult);

            // Act
            var result = await _productsController.Get(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResponse = Assert.IsType<ErrorResponseDto>(badRequestResult.Value);
            Assert.Equal((int)HttpStatusCode.BadRequest, errorResponse.Status);
            Assert.Single(errorResponse.Errors);
            Assert.Equal("Product not found", errorResponse.Errors.First().Message);
        }

        #endregion

        #region Post

        [Fact]
        public async Task Post_ValidProduct_ReturnsCreatedAtActionResultWithData()
        {
            // Arrange
            var createProductDto = new CreateProductDto("Product1", "Description", 5, (decimal)10.0);
            var expectedResult = new Product(1, "Product1", "Description", 5, (decimal)10.0);
            _productServiceMock.Setup(x => x.Create(createProductDto)).ReturnsAsync(Result.Ok(expectedResult));

            // Act
            var result = await _productsController.Post(createProductDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_productsController.Post), createdAtActionResult.ActionName);
            Assert.Equal(expectedResult, createdAtActionResult.Value);
        }

        [Fact]
        public async Task Post_InvalidProduct_ReturnsBadRequestWithError()
        {
            // Arrange
            var createProductDto = new CreateProductDto("InvalidProduct", "Description", 0, (decimal)0.0);
            var expectedResult = Result.Fail("Invalid product");
            _productServiceMock.Setup(x => x.Create(createProductDto)).ReturnsAsync(expectedResult);

            // Act
            var result = await _productsController.Post(createProductDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResponse = Assert.IsType<ErrorResponseDto>(badRequestResult.Value);
            Assert.Equal((int)HttpStatusCode.BadRequest, errorResponse.Status);
            Assert.Single(errorResponse.Errors);
        }

        #endregion

        #region Put

        [Fact]
        public async Task Put_ValidIdAndProduct_ReturnsOkResultWithData()
        {
            // Arrange
            var updateProductDto = new UpdateProductDto("UpdatedProduct", "UpdatedDescription", 10, (decimal)20.0);
            var expectedResult = new Product(1, "UpdatedProduct", "UpdatedDescription", 10, (decimal)20.0);
            _productServiceMock.Setup(x => x.Update(1, updateProductDto)).ReturnsAsync(Result.Ok(expectedResult));

            // Act
            var result = await _productsController.Put(1, updateProductDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedResult, okResult.Value);
        }

        [Fact]
        public async Task Put_InvalidIdAndProduct_ReturnsBadRequestWithError()
        {
            // Arrange
            var updateProductDto = new UpdateProductDto("InvalidProduct", "UpdatedDescription", 10, (decimal)20.0);
            var expectedResult = Result.Fail("Product not found");
            _productServiceMock.Setup(x => x.Update(0, updateProductDto)).ReturnsAsync(expectedResult);

            // Act
            var result = await _productsController.Put(0, updateProductDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResponse = Assert.IsType<ErrorResponseDto>(badRequestResult.Value);
            Assert.Equal((int)HttpStatusCode.BadRequest, errorResponse.Status);
            Assert.Single(errorResponse.Errors);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task Delete_ValidId_ReturnsNoContentResult()
        {
            // Arrange
            var expectedResult = Result.Ok();
            _productServiceMock.Setup(x => x.Delete(1)).ReturnsAsync(expectedResult);

            // Act
            var result = await _productsController.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_InvalidId_ReturnsBadRequestWithError()
        {
            // Arrange
            var expectedResult = Result.Fail("Product not found");
            _productServiceMock.Setup(x => x.Delete(0)).ReturnsAsync(expectedResult);

            // Act
            var result = await _productsController.Delete(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResponse = Assert.IsType<ErrorResponseDto>(badRequestResult.Value);
            Assert.Equal((int)HttpStatusCode.BadRequest, errorResponse.Status);
            Assert.Single(errorResponse.Errors);
        }

        #endregion
    }
}
