using CourseWork.Common.database.Base_Repository;
using CourseWork.Modules.Blogs.Entity;
using CourseWork.Modules.Votes.Entity;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Modules.Blogs.Repository
{

    public class BlogRepository : BaseRepository<BlogEntity>
    {
        // private readonly MyAppDbContext _context;
        private readonly ILogger<BlogRepository> _logger;
        public BlogRepository(MyAppDbContext context, ILogger<BlogRepository> logger) : base(context)
        {
            // _context = context;
            _logger = logger;
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
