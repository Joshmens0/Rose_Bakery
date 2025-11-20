using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Rose_Bakery.Dto.Request;
using Rose_Bakery.Service.Interface;

namespace Rose_Bakery.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController(ICategoryService categoryService) : ControllerBase
    {
        private readonly ICategoryService _categoryService = categoryService;

        [HttpPost("create-category")]
        public async Task<IActionResult> CreateCategoryAsync([FromBody]CreateCategoryRequestDto request)
        {
            var response=await _categoryService.CreateCategoryAsync(request);
            return Ok(response);
        }
        [HttpGet("get-all-categories")]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            var response = await _categoryService.GetAllCategoriesAsync();
            return Ok(response);
        }

        [HttpPut("update-category")]
        public async Task<IActionResult> UpdateCategoryAsync([FromBody]UpdateCategoryRequestDto request)
        {
            var response = await _categoryService.UpdateCategoryAsync(request);
            return Ok(response);
        }
        [HttpDelete("delete-category")]
        public async Task<IActionResult> DeleteCategoryAsync(string request)
        {
            var response = await _categoryService.DeleteCategoryAsync(request);
            return Ok(response);
        }
    }
}
