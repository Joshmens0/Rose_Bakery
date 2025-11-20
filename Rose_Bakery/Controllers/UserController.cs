using Microsoft.AspNetCore.Mvc;
using Rose_Bakery.Dto.Request;
using Rose_Bakery.Service.Interface;

namespace Rose_Bakery.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService userService):ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest request)
        {
            var result = await _userService.CreateUserAsync(request);
            if (result.StatusCode==StatusCodes.Status200OK)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
