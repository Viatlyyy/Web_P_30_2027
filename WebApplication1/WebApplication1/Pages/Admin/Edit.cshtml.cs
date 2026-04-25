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
        public InputModel Input { get; set; }

        public SelectList RolesSelectList { get; set; }

        public class InputModel
        {
            public string Id { get; set; }

            [Required(ErrorMessage = "Email обязателен")]
            [EmailAddress]
            public string Email { get; set; }

            public string? FirstName { get; set; }
            public string? LastName { get; set; }

            [Display(Name = "Роль")]
            public string? Role { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Новый пароль")]
            public string? NewPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null) return NotFound();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            Input = new InputModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            var currentRoles = await _userManager.GetRolesAsync(user);
            Input.Role = currentRoles.FirstOrDefault();

            var roles = await Task.Run(() => _roleManager.Roles.Select(r => r.Name).ToList());
            RolesSelectList = new SelectList(roles);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.FindByIdAsync(Input.Id);
            if (user == null) return NotFound();

            if (!ModelState.IsValid)
            {
                var roles = await Task.Run(() => _roleManager.Roles.Select(r => r.Name).ToList());
                RolesSelectList = new SelectList(roles);
                return Page();
            }

            user.Email = Input.Email;
            user.UserName = Input.Email;
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }

            
            var currentRoles = await _userManager.GetRolesAsync(user);
            var roleToAdd = Input.Role;
            var roleToRemove = currentRoles.FirstOrDefault();
            if (roleToAdd != roleToRemove)
            {
                if (!string.IsNullOrEmpty(roleToRemove))
                    await _userManager.RemoveFromRoleAsync(user, roleToRemove);
                if (!string.IsNullOrEmpty(roleToAdd))
                    await _userManager.AddToRoleAsync(user, roleToAdd);
            }

            
            if (!string.IsNullOrEmpty(Input.NewPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetResult = await _userManager.ResetPasswordAsync(user, token, Input.NewPassword);
                if (!resetResult.Succeeded)
                {
                    foreach (var error in resetResult.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                    return Page();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}