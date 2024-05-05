using CourseWork.Common.database.Base_Model;

namespace CourseWork.Common.Database.Base_Entity
{
    public class BaseUserEntity : BaseEntity
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }

        public  bool? IsActive { get; set; }

        public BaseUserEntity()
        {
            IsActive = false;
        }
    }
}
