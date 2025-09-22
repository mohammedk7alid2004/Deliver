using Deliver.BLL.DTOs.Account;
using Deliver.Dal.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deliver.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<Result<TokenDTO>> LoginAsync(LoginDTO loginDto);
        Task<Result<TokenDTO>> RegisterAsync(RegisterDTO registerDto);
        Task<Result<TokenDTO>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);

    }

}
