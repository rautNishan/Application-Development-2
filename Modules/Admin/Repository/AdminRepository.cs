using CourseWork.Common.database.Base_Repository;
using CourseWork.Modules.Admin.Entity;

namespace CourseWork.Modules.Admin.Repository
{
    public class AdminRepository : BaseRepository<AdminEntity>
    {

        public AdminRepository(MyAppDbContext context) : base(context)
        {
        }
    }
}
