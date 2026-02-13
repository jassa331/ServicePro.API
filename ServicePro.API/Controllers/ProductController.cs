using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicePro.Core.DTOs;
using ServicePro.Core.Interfaces;

namespace ServicePro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDTO dto)
        {
            var result = await _service.CreateProductAsync(dto);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var result = await _service.GetAllProductsAsync();
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("by-category")]
        public async Task<IActionResult> GetProductsByCategory()
        {
            var result = await _service.GetProductsByCategoryAsync();
            return Ok(result);
        }


    }

}
