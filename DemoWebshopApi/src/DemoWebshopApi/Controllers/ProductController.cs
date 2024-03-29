﻿using AutoMapper;
using DemoWebshopApi.Data.Entities;
using DemoWebshopApi.DTOs.RequestModels;
using DemoWebshopApi.DTOs.ResponseModels;
using DemoWebshopApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebshopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseController
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

            return Ok(_mapper.Map<ICollection<ProductResponseDto>>(products));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ProductResponseDto>> GetProduct(Guid id)
        {
            var product = await _productService.GetProduct(id);

            return _mapper.Map<ProductResponseDto>(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductResponseDto>> CreateProduct(ProductRequestDto product)
        {
            var newProduct = await _productService.CreateProduct(_mapper.Map<Product>(product));

            return CreatedAtAction("GetProduct", new { id = newProduct.Id }, _mapper.Map<ProductResponseDto>(newProduct));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(Guid id, ProductRequestDto product)
        {
            var updatedProduct = _mapper.Map<Product>(product);
            updatedProduct.Id = id;

            await _productService.UpdateProduct(id, _mapper.Map<Product>(updatedProduct));

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await _productService.DeleteProduct(id);

            return NoContent();
        }
    }
}
