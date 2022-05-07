using AirConditionerSystem.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirConditionerSystem.Models.ServiceRequests
{
    public class GetAllServiceRequestsForAdminListViewModel
    {
        public IEnumerable<GetAllServiceRequestsForAdminViewModel> Requests { get; set; }

        public FilterUserInputModel Filter { get; set; }
    }
}
