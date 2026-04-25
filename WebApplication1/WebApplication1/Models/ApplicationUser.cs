using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? AvatarPath { get; set; } 
        
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}