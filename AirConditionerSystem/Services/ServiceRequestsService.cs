using AirConditionerSystem.Data;
using AirConditionerSystem.Data.Enumerations;
using AirConditionerSystem.Models.ServiceRequests;
using AirConditionerSystem.Services.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirConditionerSystem.Services
{
    public class ServiceRequestsService : IServiceRequestsService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly Cloudinary cloudinary;

        public ServiceRequestsService(ApplicationDbContext dbContext, Cloudinary cloudinary)
        {
            this.dbContext = dbContext;
            this.cloudinary = cloudinary;
        }

        public async Task CreateAsync(CreateServiceRequestInputModel input)
        {
            string remoteUrl = string.Empty;

            if (input.Image != null)
            {
                string fileName = input.Name + Guid.NewGuid().ToString();
                remoteUrl = await this.UploadImageAsync(input.Image, fileName);
            }
            else
            {
                remoteUrl = "https://tse4.mm.bing.net/th?id=OIP.LWXa8_ZDBrj_czseO33ccAHaJY&pid=Api&P=0&w=125&h=158";
            }

            ServiceRequest serviceRequest = new ServiceRequest
            {
                Name = input.Name,
                Description = input.Description,
                Address = input.Address,
                Status = input.Status,
                CreatorId = input.CreatorId,
                Image = remoteUrl,
            };

            await this.dbContext.ServiceRequests.AddAsync(serviceRequest);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<AllServiceRequestsByCreatorIdViewModel>> GetAllByCreatorIdAsync(string creatorId)
        {
            return await this.dbContext.ServiceRequests
                .Where(s => s.CreatorId == creatorId)
                .OrderBy(s => s.Id)
                .Select(s => new AllServiceRequestsByCreatorIdViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Address = s.Address,
                    Image = s.Image,
                    Status = s.Status,
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<AllServiceRequestsByCreatorIdViewModel>> GetAllByCreatorIdAndStatusAsync(string creatorId, Status status)
        {
            return await this.dbContext.ServiceRequests
               .Where(s => s.CreatorId == creatorId && s.Status == status)
               .OrderBy(s => s.Id)
               .Select(s => new AllServiceRequestsByCreatorIdViewModel
               {
                   Id = s.Id,
                   Name = s.Name,
                   Description = s.Description,
                   Address = s.Address,
                   Image = s.Image,
                   Status = s.Status,
               })
               .ToListAsync();
        }

        private async Task<string> UploadImageAsync(IFormFile formFile, string fileName)
        {
            byte[] destinationData;

            using (var ms = new System.IO.MemoryStream())
            {
                await formFile.CopyToAsync(ms);
                destinationData = ms.ToArray();
            }

            UploadResult result = null;

            using (var ms = new System.IO.MemoryStream(destinationData))
            {
                ImageUploadParams uploadParams = new ImageUploadParams
                {
                    Folder = "exam-images",
                    File = new FileDescription(fileName, ms),
                };

                result = this.cloudinary.Upload(uploadParams);
            }

            return result?.SecureUrl.AbsoluteUri;
        }

        public async Task UpdateAsync(EditServiceRequestInputModel input)
        {
            ServiceRequest request = await this.dbContext.ServiceRequests.FirstOrDefaultAsync(s => s.Id == input.Id);

            string remoteUrl = string.Empty;

            if (input.Image != null)
            {
                string fileName = input.Name + Guid.NewGuid().ToString();
                remoteUrl = await this.UploadImageAsync(input.Image, fileName);
            }
            else
            {
                remoteUrl = request.Image;
            }

            request.Name = input.Name;
            request.Description = input.Description;
            request.Address = input.Address;
            request.Image = remoteUrl;

            await this.dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            ServiceRequest request = await this.dbContext.ServiceRequests.FirstOrDefaultAsync(s => s.Id == id);
            this.dbContext.ServiceRequests.Remove(request);

            await this.dbContext.SaveChangesAsync();
        }

        public async Task<EditServiceRequestInputModel> GetToBeUpdatedByCustomerAsync(int id)
        {
            return await this.dbContext.ServiceRequests
                .Where(s => s.Id == id)
                .Select(s => new EditServiceRequestInputModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Address = s.Address,
                    ImageUrl = s.Image,
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<GetAllServiceRequestsForAdminViewModel>> GetAllForAdminAsync()
        {
            return await this.dbContext.ServiceRequests
               .OrderBy(s => s.Id)
               .Select(s => new GetAllServiceRequestsForAdminViewModel
               {
                   Id = s.Id,
                   Name = s.Name,
                   Description = s.Description,
                   Address = s.Address,
                   Image = s.Image,
                   Status = s.Status,
                   Visitor = s.VisitedById == null ? "Not assigned" : s.VisitedBy.UserName,
                   VisitedOn = s.VisitedOn,
               })
               .ToListAsync();
        }

        public async Task<IEnumerable<GetAllServiceRequestsForAdminViewModel>> GetAllForAdminFilteredAsync(string userId)
        {
            return await this.dbContext.ServiceRequests
               .Where(s => s.CreatorId == userId)
               .OrderBy(s => s.Id)
               .Select(s => new GetAllServiceRequestsForAdminViewModel
               {
                   Id = s.Id,
                   Name = s.Name,
                   Description = s.Description,
                   Address = s.Address,
                   Image = s.Image,
                   Status = s.Status,
                   Visitor = s.VisitedById == null ? "Not assigned" : s.VisitedBy.UserName,
                   VisitedOn = s.VisitedOn,
               })
               .ToListAsync();
        }

        public async Task AssignAsync(int id, AssignRequestToTechInputModel input)
        {
            if (input.VisitedOn < DateTime.UtcNow)
            {
                throw new ArgumentException("Date should be present!");
            }

            ServiceRequest request = await this.dbContext.ServiceRequests.FirstOrDefaultAsync(s => s.Id == id);

            request.VisitedById = input.TechId;
            request.VisitedOn = input.VisitedOn;

            await this.dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<AllServiceRequestsByCreatorIdViewModel>> GetAllForTechAsync(string techId)
        {
            return await this.dbContext.ServiceRequests
               .Where(s => s.VisitedById == techId)
               .OrderBy(s => s.Id)
               .Select(s => new AllServiceRequestsByCreatorIdViewModel
               {
                   Id = s.Id,
                   Name = s.Name,
                   Description = s.Description,
                   Address = s.Address,
                   Image = s.Image,
                   Status = s.Status,
               })
               .ToListAsync();
        }

        public async Task<IEnumerable<AllServiceRequestsByCreatorIdViewModel>> GetAllForTechTodayAsync(string techId)
        {
            return await this.dbContext.ServiceRequests
               .Where(s => s.VisitedById == techId && s.VisitedOn.Day == DateTime.UtcNow.Day && s.VisitedOn.Month == DateTime.UtcNow.Month && s.VisitedOn.Year == DateTime.UtcNow.Year)
               .OrderBy(s => s.Id)
               .Select(s => new AllServiceRequestsByCreatorIdViewModel
               {
                   Id = s.Id,
                   Name = s.Name,
                   Description = s.Description,
                   Address = s.Address,
                   Image = s.Image,
                   Status = s.Status,
               })
               .ToListAsync();
        }

        public async Task UpdateStatusAsync(int id, Status status)
        {
            ServiceRequest request = await this.dbContext.ServiceRequests.FirstOrDefaultAsync(s => s.Id == id);
            request.Status = status;
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<EditStatusInputModel> GetEditStatusById(int id)
        {
            return await this.dbContext.ServiceRequests
                .Where(s => s.Id == id)
                .Select(s => new EditStatusInputModel
                {
                    Id = s.Id,
                    Status = s.Status,
                })
                .FirstOrDefaultAsync();
        }
    }
}
