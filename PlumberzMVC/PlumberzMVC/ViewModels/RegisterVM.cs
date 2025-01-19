using System.ComponentModel.DataAnnotations;

namespace PlumberzMVC.ViewModels
{
    public class RegisterVM
    {
        [Required, MaxLength(32), MinLength(9, ErrorMessage = "Full name 9 dan cox olsun gede")]
        public string FullName { get; set; } = null!;
        [Required, MaxLength(32), MinLength(3, ErrorMessage = "Full name 9 dan cox olsun gede")]
        public string UserName { get; set; } = null!;
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Required, DataType(DataType.Password), Compare("Password")]
        public string RePassword { get; set; } = null!;
    }
}
