using AirConditionerSystem.Data.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirConditionerSystem.Data
{
    public class ServiceRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public string Image { get; set; }

        public Status Status { get; set; }

        public DateTime VisitedOn { get; set; }

        public string VisitedById { get; set; }

        public virtual ApplicationUser VisitedBy { get; set; }

        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }
    }
}
