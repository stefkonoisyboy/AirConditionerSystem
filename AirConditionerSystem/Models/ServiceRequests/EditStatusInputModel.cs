using AirConditionerSystem.Data.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirConditionerSystem.Models.ServiceRequests
{
    public class EditStatusInputModel
    {
        public int Id { get; set; }

        public Status Status { get; set; }
    }
}
