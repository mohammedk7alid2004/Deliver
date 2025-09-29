using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deliver.BLL.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<bool>> GetLocationAsync(int userid,AddressDTo request)
        {
            var result=await _userRepository.AddAddress(userid, request.Government,request.City,request.Zone,request.Street);
                return Result.Success(true);
        }
    }
}
