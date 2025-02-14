using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models.Entities;
using ProductService.Models.RequestModels;
using ProductService.Services.IRepositories;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [Authorize(Policy ="Admin")]
        [HttpPost]
        [Route("token")]
        public IActionResult Get()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return Ok(claims);
        }
        // Post: api/Product/create
        [Authorize(Policy ="Admin")]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateProduct([FromBody]CreateProductRequest requestModel)
        {
            var productDTO=await _productRepository.CreateProduct(requestModel);
            if(productDTO==null){
                return BadRequest("An error occured while creating the product, debug for further knowledge");
            }
            return Ok(productDTO);
        }
        // Put: api/Product/update
        [Authorize(Policy ="Admin")]
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequest requestModel)
        {
            var product=await _productRepository.UpdateProduct(requestModel);
            if(product==null){
                return BadRequest("An error occured while creating the product, debug for further knowledge");
            }
            return Ok(product);
        }
        // Delete: api/Product/delete/id
        [Authorize(Policy ="Admin")]
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteProduct([FromQuery] int id){
            var product=await _productRepository.GetProductById(id);
            if(product==null) return BadRequest("Product does not exists");
            await _productRepository.DeleteProduct(product);
            return Ok("The product is deleted");
        }
        [HttpPost]
        [Route("GetProductNamesByIds")]
        public async Task<IActionResult> GetProductNamesByIds([FromBody] CustomerRequest request)
        {
            return Ok(await _productRepository.GetProductsByIds(request.productIds));
        }
    }
}
