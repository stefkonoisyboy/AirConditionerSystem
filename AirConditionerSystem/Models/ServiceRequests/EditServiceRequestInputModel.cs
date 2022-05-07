using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirConditionerSystem.Models.ServiceRequests
{
    public class EditServiceRequestInputModel : BaseServiceRequestInputModel
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }
    }
}
