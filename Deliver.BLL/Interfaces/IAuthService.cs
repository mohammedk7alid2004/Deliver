using Deliver.BLL.DTOs.Email;
using Microsoft.AspNetCore.Identity.Data;
using ResendConfirmationEmailRequest = Deliver.BLL.DTOs.Email.ResendConfirmationEmailRequest;
namespace Deliver.BLL.Interfaces;

public interface IAuthService
{
    Task<Result<TokenDTO>> LoginAsync(LoginDTO loginDto);
    Task<Result> RegisterAsync(RegisterDTO registerDto);
    Task<Result<TokenDTO>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
    Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request);
    Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request);
    Task<Result> VerifyResetOtpAsync(string email, string code);


}
