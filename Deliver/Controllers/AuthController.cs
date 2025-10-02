using Deliver.BLL.Interfaces;
using Deliver.Entities.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
namespace Deliver.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            var response = await _authService.LoginAsync(loginDto);

            return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDto)
        {
            var response = await _authService.RegisterAsync(registerDto);
            return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
        }



        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

            return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();
        }
    }


    [Route("account")]
    public class AccountController : Controller
    {
        [HttpGet("login")]
        public IActionResult Login()
        {
            var props = new AuthenticationProperties { RedirectUri = "/account/google-response" };
            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        [Authorize]
        [HttpGet("google-response")]
        public IActionResult GoogleResponse()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value });
            return Ok(claims);
        }
    }
}
