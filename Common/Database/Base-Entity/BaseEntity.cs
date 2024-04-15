using CourseWork.Common.database.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace CourseWork.Common.database.Base_Model
{
    public class BaseEntity : IBaseModelInterface
    {
        [Key]
        public int id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }


        //Initalizing Default Value
        public BaseEntity()
        {
            CreatedAt = DateTime.UtcNow; 
            UpdatedAt = DateTime.UtcNow; 
            DeletedAt = null;
        }

    }
}
