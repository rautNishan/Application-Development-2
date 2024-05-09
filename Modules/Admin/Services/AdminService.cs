using System.Net;
using CourseWork.Common.Constants.Enums;
using CourseWork.Common.Exceptions;
using CourseWork.Common.Middlewares.Response;
using CourseWork.Modules.Admin.Dtos;
using CourseWork.Modules.Admin.Entity;
using CourseWork.Modules.Admin.Repository;

namespace CourseWork.Modules.Admin.Services
{
    public class AdminService
    {
        private readonly AdminRepository _adminRepo;
        private readonly ILogger<AdminService> _logger;
        public AdminService(AdminRepository adminRepo, ILogger<AdminService> logger)
        {
            _adminRepo = adminRepo;
            _logger = logger;
        }


        public async Task SeedAdmin()
        {
            AdminEntity? adminExists = await _adminRepo.FindOne(a => a.UserName == "admin");
            _logger.LogInformation("Running Admin Seed");
            if (adminExists == null)
            {
                _logger.LogInformation("Running Admin Seed");
                var admin = new AdminEntity
                {
                    UserName = "admin",
                    Password = BCrypt.Net.BCrypt.HashPassword("admin")
                };
                _logger.LogInformation("Running Admin Seed");
                await _adminRepo.CreateAsync(admin);
                _logger.LogInformation("Running Admin Seed Complete");
                return;
            }
            return;
        }

        async public Task<AdminEntity?> FindOne(string userName)
        {
            return await _adminRepo.FindOne(x => x.UserName == userName);
        }

        public async Task<AdminEntity?> GetUserByIdAsync(int id)
        {
            return await _adminRepo.FindByIdAsync(id);
        }

        public async Task<AdminEntity> RegisterAdmin(AdminCreateDto incomingData)
        {
            AdminEntity? adminExists = await this.FindOne(incomingData.UserName);
            if (adminExists != null)
            {
                throw new HttpException(HttpStatusCode.Conflict, "Admin already exists");
            }

            AdminEntity adminEntity = new AdminEntity
            {
                UserName = incomingData.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword(incomingData.Password)
            };
            return await _adminRepo.CreateAsync(adminEntity);
        }

        public async Task<PaginatedResponse<AdminEntity>> GetPaginatedBlogList(int pageNumber, ShortByEnum shortBy)
        {
            PaginatedResponse<AdminEntity> results = await _adminRepo.GetAllPaginatedAsync(pageNumber, shortBy);
            return results;
        }

        public async Task<AdminEntity> UpdateAdmin(AdminEntity adminEntity, AdminUpdateDto incomingData)
        {
            if (incomingData.Password != null)
            {
                adminEntity.Password = BCrypt.Net.BCrypt.HashPassword(incomingData.Password);
            }
            return await _adminRepo.UpdateAsync(adminEntity);
        }

        public async Task<AdminEntity> SoftDeleteBlog(int id)
        {
            AdminEntity? existingAdmin = await _adminRepo.FindByIdAsync(id);
            if (existingAdmin == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Admin with that id was not found");
            }
            existingAdmin.DeletedAt = DateTime.Now;
            return await _adminRepo.SoftDeleteAsync(existingAdmin);
        }

        public async Task<AdminEntity> RestoreBlog(int id)
        {
            AdminEntity? existingAdmin = await _adminRepo.FindByIdIncludingDeletedAsync(id);
            if (existingAdmin == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Admin with that id was not found");
            }
            existingAdmin.DeletedAt = null;
            return await _adminRepo.UpdateAsync(existingAdmin);
        }

        public async Task<AdminEntity> HardDelete(int id)
        {
            AdminEntity? existingAdmin = await _adminRepo.FindByIdIncludingDeletedAsync(id);
            if (existingAdmin == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Admin with that id was not found");
            }
            return await _adminRepo.DeleteAsync(existingAdmin);
        }

    }
}
