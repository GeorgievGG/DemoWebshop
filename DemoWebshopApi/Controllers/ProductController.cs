using AutoMapper;
using DemoWebshopApi.Data.Entities;
using DemoWebshopApi.DTO.RequestModels;
using DemoWebshopApi.DTO.ResponseModels;
using DemoWebshopApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebshopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public ProductController(IMapper mapper, IProductService productService)
        {
            _mapper = mapper;
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetProducts()
        {
            var products = await _productService.GetProducts();
            if (products == null)
            {
                return NotFound();
            }

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDto>> GetProduct(Guid id)
        {
            var product = await _productService.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }

            return _mapper.Map<ProductResponseDto>(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, ProductRequestDto product)
        {
            var updatedProduct = _mapper.Map<Product>(product);
            updatedProduct.Id = id;

            var isSuccessful = await _productService.UpdateProduct(id, _mapper.Map<Product>(updatedProduct));
            if (!isSuccessful)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> CreateProduct(ProductRequestDto product)
        {
            var newProduct = await _productService.CreateProduct(_mapper.Map<Product>(product));

            return CreatedAtAction("GetProduct", new { id = newProduct.Id }, _mapper.Map<ProductResponseDto>(newProduct));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var isSuccessful = await _productService.DeleteProduct(id);
            if (!isSuccessful)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
