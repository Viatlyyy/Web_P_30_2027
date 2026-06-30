using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using WebApplication1.Data;
using WebApplication1.Hubs;
using CourseModel = WebApplication1.Models.Course;
using System.Threading.Tasks;

namespace WebApplication1.Pages.Courses
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<AppHub> _hubContext;

        public DeleteModel(ApplicationDbContext context, IHubContext<AppHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [BindProperty]
        public CourseModel Course { get; set; } = new CourseModel();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            var course = await _context.Courses.FirstOrDefaultAsync(m => m.Id == id);
            if (course == null) return NotFound();
            Course = course;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("DataChanged", "Course", "Delete");
            }
            return RedirectToPage("./Index");
        }
    }
}