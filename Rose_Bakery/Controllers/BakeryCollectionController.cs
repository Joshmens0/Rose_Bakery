using Microsoft.AspNetCore.Mvc;
using Rose_Bakery.Service.Interface;

namespace Rose_Bakery.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BakeryCollectionController(IBakeryCollectionService service):ControllerBase
    {
        private readonly IBakeryCollectionService _service=service;

        [HttpGet]
        public async Task<IActionResult> GetBakeryCollectionAsync()
        {
            var response=await _service.GetBakeryCollectionAsync();
            return Ok(response);
        }
    }

}
