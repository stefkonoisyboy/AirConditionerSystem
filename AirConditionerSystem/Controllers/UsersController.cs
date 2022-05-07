using AirConditionerSystem.Data;
using AirConditionerSystem.Models.Users;
using AirConditionerSystem.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirConditionerSystem.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(IUsersService usersService, UserManager<ApplicationUser> userManager)
        {
            this.usersService = usersService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> All()
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            if (user.Role != GlobalConstants.AdministratorRoleName)
            {
                this.TempData["Error"] = "You should be Administrator to execute this action!";
                this.Redirect("/");
            }

            IEnumerable<AllUserViewModel> viewModel = await this.usersService.GetAllAsync();
            return this.View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            if (user.Role != GlobalConstants.AdministratorRoleName)
            {
                this.TempData["Error"] = "You should be Administrator to execute this action!";
                this.Redirect("/");
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserInputModel input)
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            if (user.Role != GlobalConstants.AdministratorRoleName)
            {
                this.TempData["Error"] = "You should be Administrator to execute this action!";
                this.Redirect("/");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.usersService.CreateAsync(input);
            this.TempData["Success"] = "User successfully created!";

            return this.RedirectToAction("All");
        }

        public async Task<IActionResult> Edit(string id)
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            if (user.Role != GlobalConstants.AdministratorRoleName)
            {
                this.TempData["Error"] = "You should be Administrator to execute this action!";
                this.Redirect("/");
            }

            EditUserInputModel input = await this.usersService.GetByIdAsync(id);

            return this.View(input);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, EditUserInputModel input)
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            if (user.Role != GlobalConstants.AdministratorRoleName)
            {
                this.TempData["Error"] = "You should be Administrator to execute this action!";
                this.Redirect("/");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            input.Id = id;
            await this.usersService.UpdateAsync(input);
            this.TempData["Success"] = "User successfully updated!";

            return this.RedirectToAction("All");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            if (user.Role != GlobalConstants.AdministratorRoleName)
            {
                this.TempData["Error"] = "You should be Administrator to execute this action!";
                this.Redirect("/");
            }

            await this.usersService.DeleteAsync(id);
            this.TempData["Success"] = "User successfully deleted!";

            return this.RedirectToAction("All");
        }
    }
}
