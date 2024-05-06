
using CourseWork.Common.Database.Base_Entity;
using System.ComponentModel.DataAnnotations;
namespace CourseWork.Modules.User.Entity
{
    public class UserEntity : BaseUserEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }

    }
}
