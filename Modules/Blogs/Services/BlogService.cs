using System.Net;
using CourseWork.Common.Constants.Enums;
using CourseWork.Common.Dtos;
using CourseWork.Common.Exceptions;
using CourseWork.Common.Middlewares.Response;
using CourseWork.Modules.Blogs.Dtos;
using CourseWork.Modules.Blogs.Entity;
using CourseWork.Modules.Blogs.Repository;
using CourseWork.Modules.user.repository;
using CourseWork.Modules.User.Entity;
using CourseWork.Modules.User.Services;
using Microsoft.OpenApi.Any;

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
                // UpVote = incomingData.UpVote,
                // DownVote = incomingData.DownVote,
                PostUser = new UserInfo { UserId = int.Parse(incomingUserInfo.UserId), Name = incomingUserInfo.Name }
            };
            return await _blogRepo.CreateAsync(blogEntity);
        }
        public async Task<BlogEntity> UpdateBlogs(BlogUpdateDto incomingData, CommonUserDto incomingUserInfo, BlogEntity blogEntity)
        {

            if (incomingData.Title != null)
            {
                blogEntity.Title = incomingData.Title;
            }
            if (incomingData.Content != null)
            {
                blogEntity.Content = incomingData.Content;
            }
            if (incomingData.ImgUrl != null)
            {
                blogEntity.ImgUrl = incomingData.ImgUrl;
            }

            //This is Never Null
            if (incomingUserInfo != null)
            {
                blogEntity.PostUser = new UserInfo { UserId = int.Parse(incomingUserInfo.UserId), Name = incomingUserInfo.Name };
            }

            return await _blogRepo.UpdateAsync(blogEntity);
        }

        //This is to call from other services
        public async Task<BlogEntity> UpdateFormOtherService(BlogEntity blogEntity)
        {
            return await _blogRepo.UpdateAsync(blogEntity);
        }
        public async Task<BlogEntity?> GetByIdAsync(int id)
        {
            return await _blogRepo.FindByIdAsync(id);
        }

        public async Task<PaginatedResponse<BlogEntity>> GetPaginatedBlogList(int pageNumber, ShortByEnum shortBy)
        {
            PaginatedResponse<BlogEntity> results = await _blogRepo.GetAllPaginatedAsync(pageNumber, shortBy);
            return results;
        }

        public async Task<BlogEntity> SoftDeleteBlog(int id)
        {
            BlogEntity? existingBlog = await this._blogRepo.FindByIdAsync(id);
            if (existingBlog == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Blog with that id was not found");
            }
            existingBlog.DeletedAt = DateTime.Now;
            return await _blogRepo.SoftDeleteAsync(existingBlog);
        }

        public async Task<BlogEntity> RestoreBlog(int id)
        {
            BlogEntity? existingBlog = await _blogRepo.FindByIdIncludingDeletedAsync(id);
            if (existingBlog == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Blog with that id was not found");
            }
            existingBlog.DeletedAt = null;
            return await _blogRepo.UpdateAsync(existingBlog);
        }

        public async Task<BlogEntity> HardDelete(int id)
        {
            BlogEntity? existingBlog = await _blogRepo.FindByIdIncludingDeletedAsync(id);
            if (existingBlog == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Blog with that id was not found");
            }
            return await _blogRepo.DeleteAsync(existingBlog);
        }
    }
}
