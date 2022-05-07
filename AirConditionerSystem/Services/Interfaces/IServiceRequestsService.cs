using AirConditionerSystem.Data.Enumerations;
using AirConditionerSystem.Models.ServiceRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirConditionerSystem.Services.Interfaces
{
    public interface IServiceRequestsService
    {
        Task CreateAsync(CreateServiceRequestInputModel input);

        Task UpdateAsync(EditServiceRequestInputModel input);

        Task DeleteAsync(int id);

        Task AssignAsync(int id, AssignRequestToTechInputModel input);

        Task UpdateStatusAsync(int id, Status status);

        Task<EditServiceRequestInputModel> GetToBeUpdatedByCustomerAsync(int id);

        Task<EditStatusInputModel> GetEditStatusById(int id);

        Task<IEnumerable<AllServiceRequestsByCreatorIdViewModel>> GetAllByCreatorIdAsync(string creatorId);

        Task<IEnumerable<AllServiceRequestsByCreatorIdViewModel>> GetAllByCreatorIdAndStatusAsync(string creatorId, Status status);

        Task<IEnumerable<GetAllServiceRequestsForAdminViewModel>> GetAllForAdminAsync();

        Task<IEnumerable<GetAllServiceRequestsForAdminViewModel>> GetAllForAdminFilteredAsync(string userId);

        Task<IEnumerable<AllServiceRequestsByCreatorIdViewModel>> GetAllForTechAsync(string techId);

        Task<IEnumerable<AllServiceRequestsByCreatorIdViewModel>> GetAllForTechTodayAsync(string techId);
    }
}
