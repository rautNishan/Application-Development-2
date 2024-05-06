using System.Net;
using CourseWork.Common.Exceptions;
using CourseWork.Modules.Admin.Entity;
using CourseWork.Modules.Admin.Services;
using CourseWork.Modules.Auth.Dtos;
using CourseWork.Modules.Auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Modules.Auth.Controllers
{
    [ApiExplorerSettings(GroupName = "admin")]
    [Tags("Auth")]
    [Route("api/admin/auth/")]
    public class AdminAuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly AdminService _adminService;

        private readonly ILogger<UserAuthController> _logger;

        public AdminAuthController(AuthService authService, AdminService adminService, ILogger<UserAuthController> logger)
        {
            _authService = authService;
            _adminService = adminService;
            _logger = logger;
        }
        [HttpPost("login")]

        async public Task<IActionResult> Login([FromBody] UserLoginDto incomingData)
        {
            try
            {

                if (incomingData.Email != null)
                {
                    throw new HttpException(HttpStatusCode.BadRequest, "Admin cannot login with email");
                }

                AdminEntity? user = await _adminService.FindOne(incomingData.UserName);
                if (user == null)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Admin Not Found");
                }

                HttpContext.Items["CustomMessage"] = "LoggedIn Successfully";

                return Ok(_authService.Login(user, incomingData, "admin"));

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
