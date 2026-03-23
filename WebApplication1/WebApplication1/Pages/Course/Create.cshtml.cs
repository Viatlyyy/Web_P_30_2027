using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Data;
using CourseModel = WebApplication1.Models.Course; 

namespace WebApplication1.Pages.Courses
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public CourseModel Course { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Course.Name))
            {
                Course.Name = Course.Title;
            }

            _context.Courses.Add(Course); 
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}