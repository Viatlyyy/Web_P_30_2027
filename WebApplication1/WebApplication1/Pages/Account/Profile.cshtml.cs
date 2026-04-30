using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Models;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace WebApplication1.Pages.Account
{
    public class ProfileModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public ProfileModel(UserManager<ApplicationUser> userManager, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _environment = environment;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public string? CurrentAvatarPath { get; set; }

        public class InputModel
        {
            public string Email { get; set; } = string.Empty;

            public string? FirstName { get; set; }
            public string? LastName { get; set; }

            [Display(Name = "Аватар")]
            public IFormFile? AvatarImage { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            Input.Email = user.Email ?? string.Empty;
            Input.FirstName = user.FirstName;
            Input.LastName = user.LastName;
            CurrentAvatarPath = user.AvatarPath;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

       
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;

   
            if (Input.AvatarImage != null && Input.AvatarImage.Length > 0)
            {
              
                if (!string.IsNullOrEmpty(user.AvatarPath))
                {
                    var oldPath = Path.Combine(_environment.WebRootPath, user.AvatarPath.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "avatars");
                Directory.CreateDirectory(uploadsFolder);
                string uniqueFileName = $"{user.Id}_{System.DateTime.Now.Ticks}_{Path.GetFileName(Input.AvatarImage.FileName)}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.AvatarImage.CopyToAsync(stream);
                }
                user.AvatarPath = $"/uploads/avatars/{uniqueFileName}";
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }

          
            CurrentAvatarPath = user.AvatarPath;
            
            return RedirectToPage();
        }
    }
}