using Microsoft.AspNetCore.Mvc;
using Rose_Bakery.Service.Interface;

namespace Rose_Bakery.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BakeryCollectionController(IBakeryCollectionService service):ControllerBase
    {
        private readonly IBakeryCollectionService _service=service;

        [HttpGet("get-product-collection")]
        public async Task<IActionResult> GetBakeryCollectionAsync()
        {
            var response=await _service.GetBakeryCollectionAsync();
            return Ok(response);
        }
        [HttpPost("get-paged-list")]
        public async Task<IActionResult> GetPagedCollection(int page,int pageSize)
        {
            var response=await _service.GetPagedCollection(page,pageSize);
            return Ok(response);
        }
    }

}
