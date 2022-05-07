using AirConditionerSystem.Models.Users;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirConditionerSystem.Services.Interfaces
{
    public interface IUsersService
    {
        Task CreateAsync(CreateUserInputModel input);

        Task UpdateAsync(EditUserInputModel input);

        Task DeleteAsync(string id);

        Task<EditUserInputModel> GetByIdAsync(string id);

        Task<IEnumerable<AllUserViewModel>> GetAllAsync(); 

        Task<IEnumerable<SelectListItem>> GetAllForDropDownAsync();

        Task<IEnumerable<SelectListItem>> GetAllTechsForDropDownASync();
    }
}
