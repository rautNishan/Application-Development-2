using CourseWork.Common.database.Base_Repository;
using CourseWork.Modules.User.Entity;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Modules.user.repository
{
    public class UserRepository:BaseRepository<UserEntity>
    {
        public UserRepository(MyAppDbContext context) :base(context) {}


    }
}
