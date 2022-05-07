using AirConditionerSystem.Data.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirConditionerSystem.Models.ServiceRequests
{
    public class AllServiceRequestsByCreatorIdViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public string Image { get; set; }

        public Status Status { get; set; }
    }
}
