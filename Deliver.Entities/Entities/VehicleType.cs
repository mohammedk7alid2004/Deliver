using Deliver.Entities.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deliver.Entities.Entities
{
    public class VehicleType:TrackingBase
    {
        public int Id { get; set; }
        public VehicleType vehicleType { get; set; } 
        public string? Description { get; set; }

        public ICollection<Delivery> Deliverys { get; set; } = [];
    }

}
