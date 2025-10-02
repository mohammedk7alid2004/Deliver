
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

namespace Deliver.BLL.Services;

public class AuthService(
    IUserRepository userRepository
    , IUnitOfWork unitOf,
    SignInManager<ApplicationUser> signInManager,
    IJwtProvider jwtProvider,
    UserManager<ApplicationUser> userManager) : IAuthService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOf = unitOf;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly int _refreshTokenExpiryDays = 14;

    public async Task<Result<TokenDTO>> LoginAsync(LoginDTO loginDto)
    {
        var user = await _userRepository.FindByEmailAsync(loginDto.Email);

        if (user == null)
            return Result.Failure<TokenDTO>(UserErrors.UserNotFound);

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded)
            return Result.Failure<TokenDTO>(UserErrors.InvalidCredentials);

        var roles = await _userRepository.GetRolesAsync(user);

        var tokenResult = _jwtProvider.GenerateToken(user);
        var newRefreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = refreshTokenExpiration
        });
        var tokenDto = new TokenDTO(
            user.Id,
            tokenResult.token,
            tokenResult.expiresIn,
            newRefreshToken,
            refreshTokenExpiration
        );
        return Result.Success(tokenDto);
    }
    public async Task<Result<TokenDTO>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var user = _jwtProvider.ValidateToken(token);
        if (user is null)
            return Result.Failure<TokenDTO>(UserErrors.InvalidJwtToken);
        var userId = await _userManager.FindByIdAsync(user);
        if (userId is null)
            return Result.Failure<TokenDTO>(UserErrors.InvalidJwtToken);
        var userRefreshToken = userId.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);
        if (userRefreshToken is null)
            return Result.Failure<TokenDTO>(UserErrors.InvalidRefreshToken);

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        var (newToken, expiresIn) = _jwtProvider.GenerateToken(userId);
        var newRefreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        userId.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = refreshTokenExpiration
        });

        await _userManager.UpdateAsync(userId);

        var response = new TokenDTO(userId.Id,  newToken, expiresIn, newRefreshToken, refreshTokenExpiration);

        return Result.Success(response);

    }
    public async Task<Result<TokenDTO>> RegisterAsync(RegisterDTO registerDto)
    {
        var exist = await _userRepository.Any(registerDto.Email);

        if (exist == true)
            return Result.Failure<TokenDTO>(UserErrors.DuplicatedEmail);


        var user = new ApplicationUser
        {
            Email = registerDto.Email,
            UserName = registerDto.Email
        };
        var result = await _userRepository.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
        {

            //TODOO
            //var role = registerDto.UserType.ToString();
            //await _userRepository.AddToRoleAsync(user, role);
            var tokenResult = _jwtProvider.GenerateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);
            user.RefreshTokens.Add(new RefreshToken
            {
                Token = newRefreshToken,
                ExpiresOn = refreshTokenExpiration
            });
            var tokenDto = new TokenDTO(
                user.Id,
                tokenResult.token,
                tokenResult.expiresIn,
                newRefreshToken,
                refreshTokenExpiration
            );


            return Result.Success(tokenDto);

        }

        var error = result.Errors.FirstOrDefault();
        return Result.Failure<TokenDTO>(new Error(error.Code, error.Description, StatusCode: StatusCodes.Status409Conflict));

    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    public async Task<Result> GetUserType(int userid,UserType userType)
    {
        await _userRepository.CreateUserProfileAsync(userid,userType);
        return Result.Success();
    }
}
