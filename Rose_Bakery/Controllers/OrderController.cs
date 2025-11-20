using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rose_Bakery.Dto.Request;
using Rose_Bakery.Service.Interface;
using System.Threading.Tasks;

namespace Rose_Bakery.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _orderService.CreateOrderAsync(request);

            if (response.StatusCode == 404)
            {
                return NotFound(response);
            }

            if (response.StatusCode == 500)
            {
                return StatusCode(500, response);
            }

            return Ok(response);
        }
        [AllowAnonymous]
        [HttpGet("get-all-orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }
    }
}
