using AirConditionerSystem.Data;
using AirConditionerSystem.Models.ServiceRequests;
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
    public class ServiceRequestsController : Controller
    {
        private readonly IServiceRequestsService serviceRequestsService;
        private readonly IUsersService usersService;
        private readonly UserManager<ApplicationUser> userManager;

        public ServiceRequestsController(
            IServiceRequestsService serviceRequestsService,
            IUsersService usersService,
            UserManager<ApplicationUser> userManager)
        {
            this.serviceRequestsService = serviceRequestsService;
            this.usersService = usersService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> GetAllForTech()
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            if (user.Role != GlobalConstants.TechRoleName)
            {
                this.TempData["Error"] = "You should be Tech to execute this action!";
                this.Redirect("/");
            }

            IEnumerable<AllServiceRequestsByCreatorIdViewModel> viewModel = await this.serviceRequestsService.GetAllForTechAsync(user.Id);
            return this.View(viewModel);
        }

        public async Task<IActionResult> GetAllForTechToday()
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            if (user.Role != GlobalConstants.TechRoleName)
            {
                this.TempData["Error"] = "You should be Tech to execute this action!";
                this.Redirect("/");
            }

            IEnumerable<AllServiceRequestsByCreatorIdViewModel> viewModel = await this.serviceRequestsService.GetAllForTechTodayAsync(user.Id);
            return this.View(viewModel);
        }

        public async Task<IActionResult> GetAllForAdmin()
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            if (user.Role != GlobalConstants.AdministratorRoleName)
            {
                this.TempData["Error"] = "You should be Administrator to execute this action!";
                this.Redirect("/");
            }

            GetAllServiceRequestsForAdminListViewModel viewModel = new GetAllServiceRequestsForAdminListViewModel
            {
                Requests = await this.serviceRequestsService.GetAllForAdminAsync(),
                Filter = new FilterUserInputModel
                {
                    UserItems = await this.usersService.GetAllForDropDownAsync(),
                },
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> GetAllForAdminFiltered(FilterUserInputModel filter)
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            if (user.Role != GlobalConstants.AdministratorRoleName)
            {
                this.TempData["Error"] = "You should be Administrator to execute this action!";
                this.Redirect("/");
            }

            GetAllServiceRequestsForAdminListViewModel viewModel = new GetAllServiceRequestsForAdminListViewModel
            {
                Requests = await this.serviceRequestsService.GetAllForAdminFilteredAsync(filter.Id),
                Filter = new FilterUserInputModel
                {
                    UserItems = await this.usersService.GetAllForDropDownAsync(),
                },
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> AllByCreator()
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            if (user.Role != GlobalConstants.CustomerRoleName)
            {
                this.TempData["Error"] = "You should be Customer to execute this action!";
                this.Redirect("/");
            }

            AllServiceRequestsByCreatorIdListViewModel viewModel = new AllServiceRequestsByCreatorIdListViewModel
            {
                Requests = await this.serviceRequestsService.GetAllByCreatorIdAsync(user.Id),
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> AllByStatusAndCreator(FilterRequestInputModel filter)
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            if (user.Role != GlobalConstants.CustomerRoleName)
            {
                this.TempData["Error"] = "You should be Customer to execute this action!";
                this.Redirect("/");
            }

            AllServiceRequestsByCreatorIdListViewModel viewModel = new AllServiceRequestsByCreatorIdListViewModel
            {
                Requests = await this.serviceRequestsService.GetAllByCreatorIdAndStatusAsync(user.Id, filter.Status),
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.User);

            EditServiceRequestInputModel input = await this.serviceRequestsService.GetToBeUpdatedByCustomerAsync(id);
            return this.View(input);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditServiceRequestInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
          
            input.Id = id;
            await this.serviceRequestsService.UpdateAsync(input);
            this.TempData["Success"] = "Service Requests successfully updated!";

            return this.Redirect("/");
        }

        public async Task<IActionResult> Create()
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
          
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateServiceRequestInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
           
            input.CreatorId = user.Id;
            input.Status = Data.Enumerations.Status.Pending;
            await this.serviceRequestsService.CreateAsync(input);
            this.TempData["Success"] = "Service Requests successfully created!";

            return this.Redirect("/");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
           
            await this.serviceRequestsService.DeleteAsync(id);
            this.TempData["Success"] = "Service Request successfully deleted!";

            return this.Redirect("/");
        }

        public async Task<IActionResult> Assign(int id)
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            if (user.Role != GlobalConstants.AdministratorRoleName)
            {
                this.TempData["Error"] = "You should be Administrator to execute this action!";
                this.Redirect("/");
            }

            AssignRequestToTechInputModel input = new AssignRequestToTechInputModel
            {
                TechItems = await this.usersService.GetAllTechsForDropDownASync(),
            };

            return this.View(input);
        }

        [HttpPost]
        public async Task<IActionResult> Assign(int id, AssignRequestToTechInputModel input)
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            if (user.Role != GlobalConstants.AdministratorRoleName)
            {
                this.TempData["Error"] = "You should be Administrator to execute this action!";
                this.Redirect("/");
            }

            try
            {
                await this.serviceRequestsService.AssignAsync(id, input);
                this.TempData["Success"] = "Request assigned successfully!";
            }
            catch (Exception ex)
            {
                this.TempData["Error"] = ex.Message;
                return this.Redirect("/");
            }

            return this.Redirect("/");
        }

        public async Task<IActionResult> EditStatus(int id)
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            if (user.Role != GlobalConstants.TechRoleName)
            {
                this.TempData["Error"] = "You should be Tech to execute this action!";
                this.Redirect("/");
            }

            EditStatusInputModel input = await this.serviceRequestsService.GetEditStatusById(id);

            return this.View(input);
        }

        [HttpPost]
        public async Task<IActionResult> EditStatus(int id, EditStatusInputModel input)
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            if (user.Role != GlobalConstants.TechRoleName)
            {
                this.TempData["Error"] = "You should be Tech to execute this action!";
                this.Redirect("/");
            }

            await this.serviceRequestsService.UpdateStatusAsync(id, input.Status);
            this.TempData["Success"] = "Request status updated successfully!";

            return this.Redirect("/");
        }
    }
}
