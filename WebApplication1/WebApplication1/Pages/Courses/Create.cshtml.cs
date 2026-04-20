using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using CourseModel = WebApplication1.Models.Course;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Pages.Courses
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public CreateModel(ApplicationDbContext context) => _context = context;

        [BindProperty]
        public CourseModel Course { get; set; }

        public SelectList InstructorsSelectList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var instructors = await _context.Instructors.ToListAsync();
            InstructorsSelectList = new SelectList(instructors, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var instructors = await _context.Instructors.ToListAsync();
                InstructorsSelectList = new SelectList(instructors, "Id", "Name");
                return Page();
            }
            if (string.IsNullOrWhiteSpace(Course.Name))
                Course.Name = Course.Title;
            _context.Courses.Add(Course);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}