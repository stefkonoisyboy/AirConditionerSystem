using AirConditionerSystem.Data;
using AirConditionerSystem.Models.Users;
using AirConditionerSystem.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirConditionerSystem.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task CreateAsync(CreateUserInputModel input)
        {
            ApplicationUser user = new ApplicationUser
            {
                Email = input.Email,
                UserName = input.Email,
                Role = input.Role,
            };

            await this.userManager.CreateAsync(user, input.Password);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            ApplicationUser user = await this.dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            await this.userManager.DeleteAsync(user);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<AllUserViewModel>> GetAllAsync()
        {
            return await this.dbContext.Users
                .OrderBy(u => u.UserName)
                .Select(u => new AllUserViewModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    Role = u.Role,
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetAllForDropDownAsync()
        {
            return await this.dbContext.Users
                .Where(u => u.Role == GlobalConstants.CustomerRoleName)
                .OrderBy(u => u.UserName)
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.UserName,
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetAllTechsForDropDownASync()
        {
            return await this.dbContext.Users
                .Where(u => u.Role == GlobalConstants.TechRoleName)
                .OrderBy(u => u.UserName)
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.UserName,
                })
                .ToListAsync();
        }

        public async Task<EditUserInputModel> GetByIdAsync(string id)
        {
            return await this.dbContext.Users
                .Where(u => u.Id == id)
                .Select(u => new EditUserInputModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    Password = u.PasswordHash,
                    Role = u.Role,
                })
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(EditUserInputModel input)
        {
            ApplicationUser user = await this.dbContext.Users.FirstOrDefaultAsync(u => u.Id == input.Id);

            user.Email = input.Email;
            user.UserName = input.Email;
            user.Role = input.Role;

            if (input.Password != null)
            {
                string newPassword = this.userManager.PasswordHasher.HashPassword(user, input.Password);
                user.PasswordHash = newPassword;
            }

            await this.dbContext.SaveChangesAsync();
        }
    }
}
