using Deliver.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deliver.Entities.Interfaces
{
    public interface IDeliveryRepository
    {
        Task<bool> GetVehicleType(int userid,VehicleType vehicleType);

    }
}
