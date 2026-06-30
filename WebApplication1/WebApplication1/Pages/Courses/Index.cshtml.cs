using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using CourseModel = WebApplication1.Models.Course;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication1.Pages.Courses
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context) => _context = context;

        public IList<CourseModel> Courses { get; set; } = new List<CourseModel>();

        public async Task OnGetAsync()
        {
            Courses = await _context.Courses.Include(c => c.Instructor).ToListAsync();
        }
    }
}