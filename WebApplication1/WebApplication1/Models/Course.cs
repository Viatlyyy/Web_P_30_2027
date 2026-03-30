using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Course : EFModel
    {
        [Required(ErrorMessage = "Поле 'Название' обязательно для заполнения")]
        public string Title { get; set; }
    }
}