using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Hubs;
using WebApplication1.Models;
using CourseModel = WebApplication1.Models.Course;

namespace WebApplication1.Pages.Courses
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<AppHub> _hubContext;

        public EditModel(ApplicationDbContext context, IHubContext<AppHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [BindProperty]
        public CourseModel Course { get; set; } = new CourseModel();
        public SelectList InstructorsSelectList { get; set; } = new SelectList(Enumerable.Empty<Instructor>(), "Id", "Name");

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();
            Course = course;
            var instructors = await _context.Instructors.ToListAsync();
            InstructorsSelectList = new SelectList(instructors, "Id", "Name", Course.InstructorId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var instructors = await _context.Instructors.ToListAsync();
                InstructorsSelectList = new SelectList(instructors, "Id", "Name", Course.InstructorId);
                return Page();
            }
            if (string.IsNullOrWhiteSpace(Course.Name))
                Course.Name = Course.Title;
            _context.Attach(Course).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("DataChanged", "Course", "Edit");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Courses.Any(c => c.Id == Course.Id)) return NotFound();
                else throw;
            }
            return RedirectToPage("./Index");
        }
    }
}