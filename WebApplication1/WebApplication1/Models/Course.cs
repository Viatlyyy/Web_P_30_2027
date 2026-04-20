using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class Course : EFModel
    {
        [Required(ErrorMessage = "Поле 'Название' обязательно для заполнения")]
        public string Title { get; set; }

        
        public int? InstructorId { get; set; }

        
        public Instructor? Instructor { get; set; }

        
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}