using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirConditionerSystem.Data
{
    public class ApplicationUser : IdentityUser<string>
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();

            this.VisitedRequests = new HashSet<ServiceRequest>();
            this.CreatedRequests = new HashSet<ServiceRequest>();
        }

        public string Role { get; set; }

        public virtual IEnumerable<ServiceRequest> VisitedRequests { get; set; }

        public virtual IEnumerable<ServiceRequest> CreatedRequests { get; set; }
    }
}
