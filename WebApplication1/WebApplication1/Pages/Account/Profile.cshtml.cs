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

        public ProfileModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public string? AvatarBase64 { get; set; }
        public string? AvatarContentType { get; set; }

        public class InputModel
        {
            public string? Email { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            [Display(Name = "Аватар")]
            public IFormFile? AvatarImage { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            Input.Email = user.Email;
            Input.FirstName = user.FirstName;
            Input.LastName = user.LastName;

            if (user.AvatarBytes != null && user.AvatarBytes.Length > 0)
            {
                AvatarBase64 = Convert.ToBase64String(user.AvatarBytes);
                AvatarContentType = user.AvatarContentType;
            }

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
                using var memoryStream = new MemoryStream();
                await Input.AvatarImage.CopyToAsync(memoryStream);
                user.AvatarBytes = memoryStream.ToArray();
                user.AvatarContentType = Input.AvatarImage.ContentType;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }

            return RedirectToPage();
        }
    }
}