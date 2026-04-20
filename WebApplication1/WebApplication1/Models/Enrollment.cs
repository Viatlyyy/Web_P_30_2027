using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Enrollment
    {
        [Required(ErrorMessage = "Поле 'Студент' обязательно для заполнения")]
        [Display(Name = "Студент")]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        [Required(ErrorMessage = "Поле 'Курс' обязательно для заполнения")]
        [Display(Name = "Курс")]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        [Required(ErrorMessage = "Поле 'Дата записи' обязательно для заполнения")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата записи")]
        public DateTime EnrollmentDate { get; set; }

        [Range(0, 100, ErrorMessage = "Оценка должна быть от 0 до 100")]
        [Display(Name = "Оценка (0-100)")]
        public int? Grade { get; set; }
    }
}