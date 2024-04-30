using CourseWork.Common.database.Base_Repository;
using CourseWork.Modules.User.Entity;


namespace CourseWork.Modules.user.repository
{
    public class UserRepository : BaseRepository<UserEntity>
    {
        public UserRepository(MyAppDbContext context) : base(context) { }


    }
}
