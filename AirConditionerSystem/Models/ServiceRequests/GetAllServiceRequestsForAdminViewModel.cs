using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirConditionerSystem.Models.ServiceRequests
{
    public class GetAllServiceRequestsForAdminViewModel : AllServiceRequestsByCreatorIdViewModel
    {
        public string Visitor { get; set; }

        public DateTime VisitedOn { get; set; }
    }
}
