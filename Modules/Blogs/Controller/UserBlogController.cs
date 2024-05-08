using System.Net;
using CourseWork.Common.Constants.Enums;
using CourseWork.Common.Dtos;
using CourseWork.Common.Exceptions;
using CourseWork.Common.Middlewares.Auth;
using CourseWork.Common.Middlewares.Response;
using CourseWork.Modules.Blogs.Dtos;
using CourseWork.Modules.Blogs.Entity;
using CourseWork.Modules.Blogs.Services;
using CourseWork.Modules.User.Entity;
using CourseWork.Modules.User.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Modules.Blogs.Controller
{
    [ApiExplorerSettings(GroupName = "user")]
    [Tags("Blogs")]
    [Route("api/user/blogs")]
    public class UserBlogController : ControllerBase
    {
        private readonly BlogService _blogService;
        private readonly ILogger<AdminBlogController> _logger;

        private readonly UserService _userService;

        public UserBlogController(BlogService blogService, ILogger<AdminBlogController> logger, UserService userService)
        {
            _blogService = blogService;
            _logger = logger;
            _userService = userService;
        }

        [HttpPost("create")]
        [ServiceFilter(typeof(RoleAuthFilter))]
        public async Task<IActionResult> CreateBlogs([FromBody] BlogCreateDto incomingData)
        {
            string userId = (HttpContext.Items["UserId"] as string)!; //Since we are using the RoleAuthFilter, we can safely assume that the UserId is a string and never null
            int parseUserId = int.Parse(userId); // Convert the string to an int
            UserEntity? user = await _userService.GetUserByIdAsync(parseUserId);

            if (user == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Admin not found");
            }
            CommonUserDto userInfo = new CommonUserDto()
            {
                UserId = user.id.ToString(),
                Name = user.Name
            };
            BlogEntity result = await _blogService.CreateBlogs(incomingData, userInfo);

            HttpContext.Items["CustomMessage"] = "Blog Created Successfully";
            return Created("", result);
        }

        [HttpGet("list")]
        [ServiceFilter(typeof(RoleAuthFilter))]
        public async Task<PaginatedResponse<BlogEntity>> GetBlogList([FromQuery] string? page = "1", [FromQuery] ShortByEnum shortBy = ShortByEnum.Latest)  //ToDoType
        {
            //Getting page from query
            //  page = HttpContext.Request.Query["page"].ToString();
            _logger.LogInformation("SortBy: " + shortBy);

            int parsePage = int.Parse(page);

            PaginatedResponse<BlogEntity> result = await _blogService.GetPaginatedBlogList(parsePage, shortBy);

            HttpContext.Items["CustomMessage"] = "Blog List Successfully";
            return result;
        }


        [HttpGet("info/{blog}")]
        [ServiceFilter(typeof(RoleAuthFilter))]
        public async Task<BlogEntity> GetBlogById(string blog)
        {
            BlogEntity? existingBlog = await _blogService.GetByIdAsync(int.Parse(blog));
            if (existingBlog == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Blog with that id was not found");
            }

            HttpContext.Items["CustomMessage"] = "Blog Successfully Get";
            return existingBlog;
        }

        [HttpPost("update/{blog}")]
        [ServiceFilter(typeof(RoleAuthFilter))]
        public async Task<BlogEntity> UpdateBlogs(string blog, [FromBody] BlogUpdateDto incomingData)
        {
            string userId = (HttpContext.Items["UserId"] as string)!; //Since we are using the RoleAuthFilter, we can safely assume that the UserId is a string and never null
            int parseUserId = int.Parse(userId); // Convert the string to an int
            UserEntity? user = await _userService.GetUserByIdAsync(parseUserId);

            if (user == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Admin not found");
            }
            CommonUserDto userInfo = new CommonUserDto()
            {
                UserId = user.id.ToString(),
                Name = user.UserName
            };

            //Check if that blog exists or not
            BlogEntity? existingBlog = await _blogService.GetByIdAsync(int.Parse(blog));
            if (existingBlog == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Blog with that id was not found");
            }

            BlogEntity result = await _blogService.UpdateBlogs(incomingData, userInfo, existingBlog);

            HttpContext.Items["CustomMessage"] = "Blog Updated Successfully";
            return result;
        }

        [HttpDelete("soft-delete/{blog}")]
        [ServiceFilter(typeof(RoleAuthFilter))]
        public async Task<BlogEntity> SoftDeleteBlog(string blog)
        {

            return await _blogService.SoftDeleteBlog(int.Parse(blog));
        }

        [HttpPost("restore/{blog}")]
        [ServiceFilter(typeof(RoleAuthFilter))]
        public async Task<BlogEntity> RestoreBlog(string blog)
        {

            return await _blogService.RestoreBlog(int.Parse(blog));
        }


        [HttpDelete("hard-delete/{blog}")]
        [ServiceFilter(typeof(RoleAuthFilter))]
        public async Task<BlogEntity> HardDelete(string blog)
        {

            return await _blogService.HardDelete(int.Parse(blog));
        }

    }
}
