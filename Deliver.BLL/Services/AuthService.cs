using Deliver.BLL.Authentication;
using Deliver.BLL.DTOs.Account;
using Deliver.BLL.Interfaces;
using Deliver.Dal.Abstractions;
using Deliver.Dal.Abstractions.Errors;
using Deliver.Entities.Entities;
using Deliver.Entities.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deliver.BLL.Services
{
    public class AuthService(IUserRepository userRepository,SignInManager<ApplicationUser> signInManager,IJwtProvider jwtProvider) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;

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

            var tokenDto = new TokenDTO(
                user.Id,
                tokenResult.token,
                tokenResult.expiresIn
            );
            return Result.Success(tokenDto);
        }

        public async Task<Result<TokenDTO>> RegisterAsync(RegisterDTO registerDto)
        {
            var exist = await _userRepository.Any(registerDto.Email);

            if (exist == true) 
                return Result.Failure<TokenDTO>(UserErrors.DuplicatedEmail);


            var user = new ApplicationUser
            {
                Email = registerDto.Email,
                UserName = registerDto.Email,
                FirstName= registerDto.FirstName,
                LastName= registerDto.LastName,
                PhoneNumber=registerDto.Phone,
                UserType=registerDto.UserType,

            };
            var result = await _userRepository.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                await _userRepository.CreateUserProfileAsync(user);

                //TODOO
                //var role = registerDto.UserType.ToString();
                //await _userRepository.AddToRoleAsync(user, role);


                var tokenResult = _jwtProvider.GenerateToken(user);
                var tokenDto = new TokenDTO(
                    user.Id,
                    tokenResult.token,
                    tokenResult.expiresIn
                );
               

                return Result.Success(tokenDto);

            }

            var error = result.Errors.FirstOrDefault();
            return Result.Failure<TokenDTO>(new Error(error.Code,error.Description,StatusCode:StatusCodes.Status409Conflict));

        }
    }
}
