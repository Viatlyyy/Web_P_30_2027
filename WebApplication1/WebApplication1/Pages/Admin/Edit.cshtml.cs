using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Pages.Admin
{
    [Authorize(Roles = "Administrator")]
    public class EditModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EditModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public SelectList RolesSelectList { get; set; } = new SelectList(Enumerable.Empty<IdentityRole>(), "Name", "Name");

        public class InputModel
        {
            [Required]
            public string Id { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? Role { get; set; }

            [DataType(DataType.Password)]
            public string? NewPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            Input.Id = user.Id;
            Input.Email = user.Email ?? string.Empty;
            Input.FirstName = user.FirstName;
            Input.LastName = user.LastName;

            var roles = await _roleManager.Roles.ToListAsync();
            RolesSelectList = new SelectList(roles, "Name", "Name");

            var userRoles = await _userManager.GetRolesAsync(user);
            Input.Role = userRoles.FirstOrDefault();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var roles = await _roleManager.Roles.ToListAsync();
                RolesSelectList = new SelectList(roles, "Name", "Name");
                return Page();
            }

            var user = await _userManager.FindByIdAsync(Input.Id);
            if (user == null)
                return NotFound();

            user.Email = Input.Email;
            user.UserName = Input.Email;
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                var rolesReload = await _roleManager.Roles.ToListAsync();
                RolesSelectList = new SelectList(rolesReload, "Name", "Name");
                return Page();
            }

            // Смена пароля, если указан
            if (!string.IsNullOrEmpty(Input.NewPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetResult = await _userManager.ResetPasswordAsync(user, token, Input.NewPassword);
                if (!resetResult.Succeeded)
                {
                    foreach (var error in resetResult.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                    var rolesReload = await _roleManager.Roles.ToListAsync();
                    RolesSelectList = new SelectList(rolesReload, "Name", "Name");
                    return Page();
                }
            }

            // Обновление роли
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!string.IsNullOrEmpty(Input.Role))
            {
                await _userManager.AddToRoleAsync(user, Input.Role);
            }

            return RedirectToPage("./Index");
        }
    }
}