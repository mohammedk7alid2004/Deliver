using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Deliver.Entities.Enums
{
    [JsonConverter(typeof(CaseInsensitiveEnumConverter<VehicleType>))]
    public enum VehicleType
    {
        Bicycle = 1,
        Car = 2,
        MotorCycle = 3,
        Scooter = 4,
        Truck = 5
        
    }

    
}
