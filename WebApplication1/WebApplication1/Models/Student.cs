using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class Student : EFModel
    {
        [Required(ErrorMessage = "Поле 'Имя' обязательно для заполнения")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле 'Фамилия' обязательно для заполнения")]
        public string LastName { get; set; } = string.Empty;

        [Range(1, 120, ErrorMessage = "Возраст должен быть от 1 до 120 лет")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Поле 'Дата рождения' обязательно для заполнения")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Поле 'Email' обязательно для заполнения")]
        [EmailAddress(ErrorMessage = "Введите корректный email адрес")]
        public string Email { get; set; } = string.Empty;

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}