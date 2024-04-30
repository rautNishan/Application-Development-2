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
    }
}
