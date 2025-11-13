using AutoMapper;
using BusinessObjects;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Services;

namespace PRN232Project.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class ProductController : ODataController
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _mapper = mapper;
        }

        [HttpGet]
        [EnableQuery]
        public IQueryable<ProductResponseDto> GetAllProducts()
        {
            var products = _productService.GetAllProductsAsync().Result;

            return products.AsQueryable();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductResponseDto>> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound("Product not found");

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> CreateProduct([FromBody] ProductRequestDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid product data.");
            }

            Product product = await _productService.CreateProductAsync(dto);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, _mapper.Map<ProductResponseDto>(product));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] ProductRequestDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid user data.");

            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound("Product not found");

            await _productService.UpdateProductAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound("Product not found");

            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
