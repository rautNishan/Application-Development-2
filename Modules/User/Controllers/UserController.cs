using CourseWork.Common.Helper.EmailService;
using CourseWork.Common.Middlewares.Auth;
using CourseWork.Modules.User.Dtos;
using CourseWork.Modules.User.Entity;
using CourseWork.Modules.User.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Modules.User.Controller
{
    [ApiExplorerSettings(GroupName = "user")] //Provides metadata about the API Explorer group that an action belongs to.
    [Tags("Users")]
    [Route("api/user/user/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }



        [HttpPost("register")]
        // [ServiceFilter(typeof(RoleAuthFilter))]
        public async Task<IActionResult> CreateUser(UserCreateDto incomingData)
        {
            try
            {
                UserEntity result = await _userService.CreateUser(incomingData);
                UserResponseDto responseData = new UserResponseDto { Id = result.id };
                // return Created($"/api/users/{result.Id}", result);
                HttpContext.Items["CustomMessage"] = "Please verify your email to continue";
                return Created("", responseData);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
