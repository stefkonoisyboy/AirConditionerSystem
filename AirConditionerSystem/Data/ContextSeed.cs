using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirConditionerSystem.Data
{
    public class ContextSeed
    {
        
        public async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(GlobalConstants.AdministratorRoleName));
            await roleManager.CreateAsync(new IdentityRole(GlobalConstants.TechRoleName));
            await roleManager.CreateAsync(new IdentityRole(GlobalConstants.CustomerRoleName));
        }
    }
}
