using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Rose_Bakery.Dto.Request;
using Rose_Bakery.Service.Interface;

namespace Rose_Bakery.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(IProductService service) : ControllerBase
    {
        private readonly IProductService _service=service;
        [HttpPost("create-product")]
        public async Task<IActionResult> CreateProductAsync(CreateProductRequestDto request)
        {
           var response=await _service.CreateProductAsync(request);
            return Ok(response);
        }
        [HttpDelete("delete-product")]
        public async Task<IActionResult> DeleteProductAsync(string request)
        {
            var response= await _service.DeleteProductAsync(request);
            return Ok(response);
        }
        [HttpGet("get-product")]
        public async Task<IActionResult> GetProductAsync([FromQuery]GetProductRequest request)
        {
            var response= await _service.GetProductAsync(request);
            return Ok(response);
        }
        [HttpGet("get-all-products")]
        public async Task<IActionResult> GetAllProductsAysnc()
        {
            var response=await _service.GetAllProductsAsync();
            return Ok(response);
        }
        [HttpPut("update-product")]
        public async Task<IActionResult> UpdateProdcutAsync([FromBody]UpdateProductRequestDto request)
        {
             var response= await _service.UpdateProductAsync(request);
            return Ok(response);
        }

    }
}
