using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deliver.BLL.Interfaces
{
    public interface IUserService
    {
        Task<Result<bool>>GetLocationAsync(int userid,AddressDTo request);    

    }
}
