﻿using System.Net;
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

            if (!string.IsNullOrEmpty(incomingData.Title))
            {
                blogEntity.Title = incomingData.Title;
            }
            if (!string.IsNullOrEmpty(incomingData.Content))
            {
                blogEntity.Content = incomingData.Content;
            }
            if (!string.IsNullOrEmpty(incomingData.ImgUrl))
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

        public async Task<GetBlogByIdResponseDto?> GetBlogWithCommentsAsync(int blogId)
        {
            return await _blogRepo.GetBlogWithCommentsAsync(blogId);
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


        //Find all Blog without pagination
        public async Task<IEnumerable<BlogEntity>> GetTopTenBlogs(int? year, int? month)
        {
            IEnumerable<BlogEntity> blogs = await _blogRepo.GetAllAsync();
            int upVoteWeightage = 2;
            int downVoteWeightage = -1;
            int commentWeightage = 1;

            if (year.HasValue && month.HasValue)
            {
                blogs = blogs.Where(b => b.CreatedAt.Year == year.Value && b.CreatedAt.Month == month.Value);
            }

            return blogs
                .OrderByDescending(b =>
                    upVoteWeightage * b.UpVote +
                    downVoteWeightage * b.DownVote +
                    commentWeightage * b.Comments.Count)
                .Take(10);
        }


        public async Task<IEnumerable<UserInfo>> GetTopTenBloggers(int? year = null, int? month = null)
        {
            IEnumerable<BlogEntity> blogs = await _blogRepo.GetAllAsync();
            int upVoteWeightage = 2;
            int downVoteWeightage = -1;
            int commentWeightage = 1;

            if (year.HasValue && month.HasValue)
            {
                blogs = blogs.Where(b => b.CreatedAt.Year == year.Value && b.CreatedAt.Month == month.Value);
            }

            return blogs
                .GroupBy(b => b.PostUser.UserId) // Group by UserId instead of PostUser
                .Select(g => new
                {
                    User = g.First().PostUser, // Get the first PostUser object from each group
                    Popularity = g.Sum(b =>
                        upVoteWeightage * b.UpVote +
                        downVoteWeightage * b.DownVote +
                        commentWeightage * b.Comments.Count)
                })
                .OrderByDescending(u => u.Popularity)
                .Take(10)
                .Select(u => u.User);
        }

        public async Task<IEnumerable<BlogEntity>> GetPersonalBlogs(UserEntity user)
        {
            IEnumerable<BlogEntity> blogs = await _blogRepo.GetAllAsyncExcludeSoftDelete();
            return blogs.Where(b => b.PostUser.UserId == user.id);
        }
    }
}
