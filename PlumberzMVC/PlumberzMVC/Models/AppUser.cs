using Microsoft.AspNetCore.Identity;

namespace PlumberzMVC.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}
