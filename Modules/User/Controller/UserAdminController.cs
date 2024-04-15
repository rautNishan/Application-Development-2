using CourseWork.Modules.User.Dtos;
using CourseWork.Modules.User.Entity;
using CourseWork.Modules.User.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Modules.User.Controller
{

    [ApiExplorerSettings(GroupName = "admin")]
    [Route("api/admin/")]
    [ApiController]
    public class UserAdminController : ControllerBase
    {
        private readonly UserService _userService;
        public UserAdminController(UserService userService)
        {
            _userService = userService;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> CreateUser(UserCreateDto incomingData)
        {
            var result = await _userService.CreateUser(incomingData);
            return Ok(result);
        }
    }
}
