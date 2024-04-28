using CourseWork.Modules.User.Dtos;
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
        public async Task<IActionResult> CreateUser(UserCreateDto incomingData)
        {
            try
            {
                var result = await _userService.CreateUser(incomingData);
                // return Created($"/api/users/{result.Id}", result);
                HttpContext.Items["CustomMessage"] = "User Created Successfully";
                return Created("", result);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
