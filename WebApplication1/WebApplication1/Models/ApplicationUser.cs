using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public byte[]? AvatarBytes { get; set; }
        public string? AvatarContentType { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AvatarPath { get; set; }
    }
}