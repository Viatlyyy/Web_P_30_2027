using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using Xunit;

namespace WebApplication1.Tests.Data
{
    public class DbContextTests
    {
        [Fact]
        public async Task Can_Add_And_Retrieve_Student()
        {
         
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_AddAndRetrieve")
                .Options;

            using var context = new ApplicationDbContext(options);
            var student = new Student
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 30,
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "john@example.com"
            };

            
            context.Students.Add(student);
            await context.SaveChangesAsync();

            var retrievedStudent = await context.Students.FirstOrDefaultAsync(s => s.Email == "john@example.com");

         
            Assert.NotNull(retrievedStudent);
            Assert.Equal("John", retrievedStudent.FirstName);
        }

        [Fact]
        public async Task Can_Add_And_Retrieve_Course()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Course")
                .Options;

            using var context = new ApplicationDbContext(options);
            var course = new Course { Title = "Math", Name = "Mathematics" };

            context.Courses.Add(course);
            await context.SaveChangesAsync();

            var retrievedCourse = await context.Courses.FirstOrDefaultAsync(c => c.Title == "Math");
            Assert.NotNull(retrievedCourse);
            Assert.Equal("Mathematics", retrievedCourse.Name);
        }
    }
}