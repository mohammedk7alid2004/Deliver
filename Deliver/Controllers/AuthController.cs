using Deliver.Api.Abstractions;
using Deliver.BLL.DTOs.Account;
using Deliver.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
