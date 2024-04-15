using CourseWork.Common.database.Base_Model;
namespace CourseWork.Modules.User.Entity
{
    public class UserEntity : BaseEntity
    {
        public required string Name { get; set; }
    }
}
