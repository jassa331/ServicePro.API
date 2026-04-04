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
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromForm] uupdateProductonlyproductstabledataDTO dto)
        {
            var result = await _service.UpdateProductAsync(id, dto);
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPatch("update-single-image")]
        public async Task<IActionResult> UpdateSingleImage([FromForm] UpdateSingleImageDTO dto)
        {
            var result = await _service.UpdateSingleImageAsync(dto);
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("get-all-products-for=listing")]
        public async Task<IActionResult> GetAllProductsForListing()
        {
            var result = await _service.GetAllProductsAsyncbyproductsandimageid();
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteproduct/{id}")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateProductStatusDto model)
        {
            var result = await _service.UpdateProductStatusAsync(id, model.IsActive);

            if (!result)
                return NotFound("Product not found");

            return Ok("Status updated successfully");
        }
        [HttpPut("restore/{id}")]
        public async Task<IActionResult> UpdateStatuss(Guid id, [FromBody] UpdateProductStatusDto model)
        {
            var result = await _service.UpdateProductStatusAsync(id, model.IsActive);

            if (!result)
                return NotFound("Product not found");

            return Ok("Status updated successfully");
        }
        [HttpGet("get-inactive-product/{id}")]
        public async Task<IActionResult> GetInactiveProduct(Guid id)
        {
            var result = await _service.getallinactiveproducts(id);
            return Ok(result);
        }
        [HttpGet("get-active-product/{id}")]
        public async Task<IActionResult> GetActiveProduct(Guid id)
        {
            var result = await _service.GetActiveProduct(id);
            return Ok(result);
        }
        [HttpGet("get-all-Product-globale-Response")]
        public async Task<IActionResult> getallProductglobale()
        {
            var result = await _service.getallProductglobaleResponseDTO();
            return Ok(result);

        }
        [HttpGet("Get-inactive-products")]
        public async Task<IActionResult> GetProduct()
        {
            var result = await _service.GetAllinactiveProductsAsync();
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
