using Deliver.Dal.Data;
using Deliver.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deliver.Dal.Repository
{
    public class DeliveryRepository(ApplicationDbContext context) : IDeliveryRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<bool> GetVehicleType(int userid, Entities.Enums.VehicleType vehicleType)
        {
            
        }
    }
}
