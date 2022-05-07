using AirConditionerSystem.Data.Enumerations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AirConditionerSystem.Models.ServiceRequests
{
    public class BaseServiceRequestInputModel
    {
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Address { get; set; }

        public string CreatorId { get; set; }

        public IFormFile Image { get; set; }

        public Status Status { get; set; }
    }
}
