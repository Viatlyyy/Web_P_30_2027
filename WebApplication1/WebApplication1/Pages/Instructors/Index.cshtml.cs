using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication1.Pages.Instructors
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context) => _context = context;

        public IList<Instructor> Instructors { get; set; } = new List<Instructor>();

        public async Task OnGetAsync()
        {
            Instructors = await _context.Instructors.Include(i => i.Courses).ToListAsync();
        }
    }
}