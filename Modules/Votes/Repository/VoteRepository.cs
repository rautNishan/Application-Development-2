using CourseWork.Common.database.Base_Repository;
using CourseWork.Modules.Votes.Entity;

namespace CourseWork.Modules.Votes.Repository
{
    public class VoteRepository : BaseRepository<VoteEntity>
    {
        public VoteRepository(MyAppDbContext context) : base(context) { }
    }
}
