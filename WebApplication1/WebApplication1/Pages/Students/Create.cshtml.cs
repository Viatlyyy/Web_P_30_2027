using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Hubs;
using System.Threading.Tasks;

namespace WebApplication1.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<AppHub> _hubContext;

        public CreateModel(ApplicationDbContext context, IHubContext<AppHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public IActionResult OnGet() => Page();

        [BindProperty]
        public Student Student { get; set; } = new Student();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            if (string.IsNullOrWhiteSpace(Student.Name))
                Student.Name = $"{Student.FirstName} {Student.LastName}";
            _context.Students.Add(Student);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("DataChanged", "Student", "Create");
            return RedirectToPage("./Index");
        }
    }
}