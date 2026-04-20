using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Pages.Enrollments
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public CreateModel(ApplicationDbContext context) => _context = context;

        [BindProperty]
        public Enrollment Enrollment { get; set; }

        public SelectList StudentsSelectList { get; set; }
        public SelectList CoursesSelectList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var students = await _context.Students.ToListAsync();
            StudentsSelectList = new SelectList(students, "Id", "Name");
            var courses = await _context.Courses.ToListAsync();
            CoursesSelectList = new SelectList(courses, "Id", "Title");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var students = await _context.Students.ToListAsync();
                StudentsSelectList = new SelectList(students, "Id", "Name");
                var courses = await _context.Courses.ToListAsync();
                CoursesSelectList = new SelectList(courses, "Id", "Title");
                return Page();
            }

           
            var exists = await _context.Enrollments
                .AnyAsync(e => e.StudentId == Enrollment.StudentId && e.CourseId == Enrollment.CourseId);
            if (exists)
            {
                ModelState.AddModelError(string.Empty, "Этот студент уже записан на данный курс.");
                return Page();
            }

            _context.Enrollments.Add(Enrollment);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}