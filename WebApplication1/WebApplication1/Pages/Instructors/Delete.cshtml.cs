using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Hubs;
using System.Threading.Tasks;

namespace WebApplication1.Pages.Instructors
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
        public Instructor Instructor { get; set; } = new Instructor();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            var instructor = await _context.Instructors.FirstOrDefaultAsync(m => m.Id == id);
            if (instructor == null) return NotFound();
            Instructor = instructor;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();
            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor != null)
            {
                _context.Instructors.Remove(instructor);
                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("DataChanged", "Instructor", "Delete");
            }
            return RedirectToPage("./Index");
        }
    }
}