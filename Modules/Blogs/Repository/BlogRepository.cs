using CourseWork.Common.database.Base_Repository;
using CourseWork.Modules.Blogs.Dtos;
using CourseWork.Modules.Blogs.Entity;
using CourseWork.Modules.Votes.Entity;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Modules.Blogs.Repository
{

    public class BlogRepository : BaseRepository<BlogEntity>
    {
        private readonly MyAppDbContext _context;
        private readonly ILogger<BlogRepository> _logger;
        public BlogRepository(MyAppDbContext context, ILogger<BlogRepository> logger) : base(context)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<GetBlogByIdResponseDto> GetBlogWithCommentsAsync(int blogId)
        {
            var blog = await _context.Blogs
                .AsNoTracking()
                .Where(b => b.id == blogId && b.DeletedAt==null)
                .Select(b => new GetBlogByIdResponseDto
                {
                    Id = b.id,
                    CreatedAt = b.CreatedAt,
                    UpdatedAt = b.UpdatedAt,
                    DeletedAt = b.DeletedAt,
                    Title = b.Title,
                    Content = b.Content,
                    PostUser = b.PostUser,
                    ImgUrl = b.ImgUrl,
                    UpVote = b.UpVote,
                    DownVote = b.DownVote,
                    Comments = b.Comments.Where(c => c.ParentCommentId == null)
                        .Select(c => new CommentDto
                        {
                            Id = c.id,
                            CommentedUserName = c.CommentedUserName,
                            Message = c.Message,
                            UpVote = c.UpVote,
                            DownVote = c.DownVote,
                            // Replies = GetReplies(c.Comments).ToList() // Assuming Comments navigation includes replies
                        }).ToList()
                }).FirstOrDefaultAsync();

            return blog;
        }
        // public async Task<BlogEntity?> GetByIdWithVotesAsync(int id)
        // {
        //     BlogEntity? data = await _context.Blogs
        //     .Include(b => b.Votes)
        //     .SingleOrDefaultAsync(b => b.id == id);
        //     return data;
        // }

    }
}
