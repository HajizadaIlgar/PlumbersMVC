using System.ComponentModel.DataAnnotations;

namespace PlumberzMVC.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string UserNameorEmail { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
