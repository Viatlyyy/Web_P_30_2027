using Microsoft.EntityFrameworkCore;
using Moq;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Pages.Students;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication1.Tests.Pages
{
    public class StudentsPageMoqTests
    {
        [Fact]
        public async Task IndexModel_OnGetAsync_GetsStudentsFromDbContext()
        {
            
            var testStudents = new List<Student>
            {
                new Student { Id = 1, FirstName = "Анна", LastName = "Иванова", Age = 20, DateOfBirth = new DateTime(2000,1,1), Email = "anna@test.com" },
                new Student { Id = 2, FirstName = "Иван", LastName = "Петров", Age = 22, DateOfBirth = new DateTime(1998,5,5), Email = "ivan@test.com" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Student>>();
            mockSet.As<IQueryable<Student>>().Setup(m => m.Provider).Returns(testStudents.Provider);
            mockSet.As<IQueryable<Student>>().Setup(m => m.Expression).Returns(testStudents.Expression);
            mockSet.As<IQueryable<Student>>().Setup(m => m.ElementType).Returns(testStudents.ElementType);
            mockSet.As<IQueryable<Student>>().Setup(m => m.GetEnumerator()).Returns(testStudents.GetEnumerator());

           
            mockSet.As<IAsyncEnumerable<Student>>()
                .Setup(m => m.GetAsyncEnumerator(CancellationToken.None))
                .Returns(new TestAsyncEnumerator<Student>(testStudents.GetEnumerator()));

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Students).Returns(mockSet.Object);

            var indexModel = new IndexModel(mockContext.Object);

            
            await indexModel.OnGetAsync();

            
            Assert.NotNull(indexModel.Students);
            Assert.Equal(2, indexModel.Students.Count);
            Assert.Equal("Анна", indexModel.Students.First().FirstName);
        }
    }

    
    public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;
        public TestAsyncEnumerator(IEnumerator<T> inner) => _inner = inner;
        public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(_inner.MoveNext());
        public T Current => _inner.Current;
        public ValueTask DisposeAsync() { _inner.Dispose(); return new ValueTask(); }
    }
}