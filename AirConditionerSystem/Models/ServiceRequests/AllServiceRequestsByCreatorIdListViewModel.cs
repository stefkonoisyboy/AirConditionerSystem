using AirConditionerSystem.Data.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirConditionerSystem.Models.ServiceRequests
{
    public class AllServiceRequestsByCreatorIdListViewModel
    {
        public IEnumerable<AllServiceRequestsByCreatorIdViewModel> Requests { get; set; }

        public FilterRequestInputModel Filter { get; set; }
    }
}
