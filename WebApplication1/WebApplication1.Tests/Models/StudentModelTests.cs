using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;
using Xunit;

namespace WebApplication1.Tests.Models
{
    public class StudentModelTests
    {
        [Fact]
        public void Student_ValidData_PassesValidation()
        {
           
            var student = new Student
            {
                FirstName = "Иван",
                LastName = "Петров",
                Age = 20,
                DateOfBirth = new DateTime(2000, 1, 1),
                Email = "ivan@example.com"
            };
            var context = new ValidationContext(student);
            var results = new List<ValidationResult>();

           
            var isValid = Validator.TryValidateObject(student, context, results, true);

            
            Assert.True(isValid);
            Assert.Empty(results);
        }

        [Fact]
        public void Student_MissingFirstName_FailsValidation()
        {
            
            var student = new Student
            {
                FirstName = null, 
                LastName = "Петров",
                Age = 20,
                DateOfBirth = new DateTime(2000, 1, 1),
                Email = "ivan@example.com"
            };
            var context = new ValidationContext(student);
            var results = new List<ValidationResult>();

         
            var isValid = Validator.TryValidateObject(student, context, results, true);

         
            Assert.False(isValid);
            Assert.Contains(results, r => r.ErrorMessage.Contains("Имя"));
        }

        [Fact]
        public void Student_InvalidEmail_FailsValidation()
        {
            
            var student = new Student
            {
                FirstName = "Иван",
                LastName = "Петров",
                Age = 20,
                DateOfBirth = new DateTime(2000, 1, 1),
                Email = "invalid-email"
            };
            var context = new ValidationContext(student);
            var results = new List<ValidationResult>();

           
            var isValid = Validator.TryValidateObject(student, context, results, true);

            
            Assert.False(isValid);
            Assert.Contains(results, r => r.ErrorMessage.Contains("Email"));
        }
    }
}