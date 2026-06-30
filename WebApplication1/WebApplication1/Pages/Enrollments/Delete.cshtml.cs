using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Hubs;
using System.Threading.Tasks;

namespace WebApplication1.Pages.Enrollments
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
        public Enrollment Enrollment { get; set; } = new Enrollment();

        public async Task<IActionResult> OnGetAsync(int studentId, int courseId)
        {
            var enrollment = await _context.Enrollments
                .Include(e => e.Student).Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);
            if (enrollment == null) return NotFound();
            Enrollment = enrollment;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int studentId, int courseId)
        {
            var enrollment = await _context.Enrollments.FindAsync(studentId, courseId);
            if (enrollment != null)
            {
                _context.Enrollments.Remove(enrollment);
                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("DataChanged", "Enrollment", "Delete");
            }
            return RedirectToPage("./Index");
        }
    }
}