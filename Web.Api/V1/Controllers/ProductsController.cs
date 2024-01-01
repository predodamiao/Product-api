using Asp.Versioning;
using Infrastructure.Dtos;
using Microsoft.AspNetCore.Mvc;
using Service.Dtos;
using Service.Services.Interfaces;

namespace Api.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Get(int pageNumber, int pageSize, string? nameToFind, string? propertyToOrderBy)
        {
            var result = await _productService.FindAll(new FindProductsDto()
            {
                NameToFind = nameToFind,
                PropertyToOrderBy = propertyToOrderBy,
                Pagination = new PaginationDto()
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                }
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _productService.GetById(id);
            return Ok(product);
        }

        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Post([FromBody] CreateProductDto productToCreate)
        {
            var createdProduct = await _productService.Create(productToCreate);
            return CreatedAtAction(nameof(Post), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPut("{id}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateProductDto productToUpdate)
        {
            var updatedProduct = await _productService.Update(id, productToUpdate);
            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.Delete(id);
            return NoContent();
        }
    }
}
