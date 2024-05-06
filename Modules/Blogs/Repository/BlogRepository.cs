using CourseWork.Common.database.Base_Repository;
using CourseWork.Modules.Blogs.Entity;

namespace CourseWork.Modules.Blogs.Repository
{
    public class BlogRepository : BaseRepository<BlogEntity>
    {
        public BlogRepository(MyAppDbContext context) : base(context) { }
    }
}
