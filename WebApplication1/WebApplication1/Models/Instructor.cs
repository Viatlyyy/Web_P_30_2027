using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class Instructor : EFModel
    {
        [Required(ErrorMessage = "Поле 'Имя' обязательно для заполнения")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Поле 'Фамилия' обязательно для заполнения")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Поле 'Отдел' обязательно для заполнения")]
        public string Department { get; set; }

        
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}