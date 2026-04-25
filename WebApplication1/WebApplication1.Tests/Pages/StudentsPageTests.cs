using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Pages.Students;
using Xunit;

namespace WebApplication1.Tests.Pages
{
    public class StudentsPageTests
    {
        [Fact]
        public async Task IndexModel_OnGetAsync_PopulatesStudentsList()
        {
          
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Students")
                .Options;

            using var context = new ApplicationDbContext(options);

            
            context.Students.Add(new Student
            {
                FirstName = "Тест",
                LastName = "Тестов",
                Age = 25,
                DateOfBirth = new DateTime(1996, 1, 1),
                Email = "test@test.com"
            });
            await context.SaveChangesAsync();

            var indexModel = new IndexModel(context);

           
            await indexModel.OnGetAsync();

           
            Assert.NotNull(indexModel.Students);
            Assert.Single(indexModel.Students);
            Assert.Equal("Тест", indexModel.Students.First().FirstName);
        }
    }
}