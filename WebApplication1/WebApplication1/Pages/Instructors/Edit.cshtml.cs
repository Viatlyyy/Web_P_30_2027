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
        public Instructor Instructor { get; set; } = new Instructor();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null) return NotFound();
            Instructor = instructor;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            Instructor.Name = $"{Instructor.FirstName} {Instructor.LastName}";
            _context.Attach(Instructor).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("DataChanged", "Instructor", "Edit");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Instructors.Any(i => i.Id == Instructor.Id)) return NotFound();
                else throw;
            }
            return RedirectToPage("./Index");
        }
    }
}