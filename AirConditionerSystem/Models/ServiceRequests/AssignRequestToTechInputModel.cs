using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirConditionerSystem.Models.ServiceRequests
{
    public class AssignRequestToTechInputModel
    {
        public string TechId { get; set; }

        public IEnumerable<SelectListItem> TechItems { get; set; }

        public DateTime VisitedOn { get; set; }
    }
}
