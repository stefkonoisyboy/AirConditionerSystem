using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirConditionerSystem.Models.Users
{
    public class FilterUserInputModel
    {
        public string Id { get; set; }

        public IEnumerable<SelectListItem> UserItems { get; set; }
    }
}
