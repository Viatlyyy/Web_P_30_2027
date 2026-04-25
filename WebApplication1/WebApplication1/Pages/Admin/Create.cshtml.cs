using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Pages.Admin
{
    [Authorize(Roles = "Administrator")]
    public class CreateModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CreateModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public SelectList RolesSelectList { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Email обязателен")]
            [EmailAddress]
            public string Email { get; set; }

            [Required(ErrorMessage = "Пароль обязателен")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Пароли не совпадают")]
            public string ConfirmPassword { get; set; }

            public string? FirstName { get; set; }
            public string? LastName { get; set; }

            [Display(Name = "Роль")]
            public string? Role { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var roles = await Task.Run(() => _roleManager.Roles.Select(r => r.Name).ToList());
            RolesSelectList = new SelectList(roles);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var roles = await Task.Run(() => _roleManager.Roles.Select(r => r.Name).ToList());
                RolesSelectList = new SelectList(roles);
                return Page();
            }

            var user = new ApplicationUser
            {
                UserName = Input.Email,
                Email = Input.Email,
                FirstName = Input.FirstName,
                LastName = Input.LastName
            };
            var result = await _userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(Input.Role))
                {
                    await _userManager.AddToRoleAsync(user, Input.Role);
                }
                return RedirectToPage("./Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            var rolesList = await Task.Run(() => _roleManager.Roles.Select(r => r.Name).ToList());
            RolesSelectList = new SelectList(rolesList);
            return Page();
        }
    }
}