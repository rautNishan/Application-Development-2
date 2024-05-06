using System.Net;
using CourseWork.Common.Dtos;
using CourseWork.Common.Exceptions;
using CourseWork.Modules.Blogs.Dtos;
using CourseWork.Modules.Blogs.Entity;
using CourseWork.Modules.Blogs.Repository;
using CourseWork.Modules.user.repository;
using CourseWork.Modules.User.Entity;
using CourseWork.Modules.User.Services;

namespace CourseWork.Modules.Blogs.Services
{
    public class BlogService
    {
        private readonly BlogRepository _blogRepo;

        private readonly UserService _userService;
        private readonly ILogger<BlogService> _logger;

        public BlogService(BlogRepository blogRepo, ILogger<BlogService> logger, UserService userService)
        {
            _blogRepo = blogRepo;
            _logger = logger;
            _userService = userService;
        }

        public async Task<BlogEntity> CreateBlogs(BlogCreateDto incomingData, CommonUserDto incomingUserInfo)
        {
            _logger.LogInformation("Creating a new blog" + incomingData);
            BlogEntity blogEntity = new BlogEntity()
            {
                Title = incomingData.Title,
                Content = incomingData.Content,
                ImgUrl = incomingData.ImgUrl,
                UpVote = incomingData.UpVote,
                DownVote = incomingData.DownVote,
                PostUser = new UserInfo { UserId = incomingUserInfo.UserId, Name = incomingUserInfo.Name }
            };
            return await _blogRepo.CreateAsync(blogEntity);
        }
    }
}
