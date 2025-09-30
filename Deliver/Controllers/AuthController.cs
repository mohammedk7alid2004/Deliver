using Deliver.BLL.DTOs.Email;
using Deliver.BLL.Interfaces;
using Deliver.BLL.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using ResendConfirmationEmailRequest = Deliver.BLL.DTOs.Email.ResendConfirmationEmailRequest;
using ResetPasswordRequest = Deliver.BLL.DTOs.Account.ResetPasswordRequest;
namespace Deliver.Api.Controllers;

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
        return response.IsSuccess ? Ok(response) : response.ToProblem();
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();
    }
    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.ConfirmEmailAsync(request);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpPost("resend-confirmation-email")]
    public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.ResendConfirmationEmailAsync(request);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPost("verify-reset-otp")]
    public async Task<IActionResult> VerifyResetOtp([FromBody] VerifyOtpRequest request)
    {
        var result = await _authService.VerifyResetOtpAsync(request.Email, request.Code);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok("OTP is valid.");
    }
    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
    {
        var result = await _authService.SendResetOtpAsync(request.Email);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var result = await _authService.ResetPasswordAsync(request);

        return result.IsSuccess ? Ok() : result.ToProblem();
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
