using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Deliver.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpGet("Set-Location")]

        public async Task<IActionResult>SetLocation(AddressDTo request)
        {
            var userid=User.GetUserId();
            var result = await _userService.GetLocationAsync(userid, request);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result);

        }

    }
}
