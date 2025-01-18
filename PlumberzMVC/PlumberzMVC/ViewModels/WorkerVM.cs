using System.ComponentModel.DataAnnotations;

namespace PlumberzMVC.ViewModels
{
    public class WorkerVM
    {
        [Required, MinLength(3, ErrorMessage = "Ad minimum 3 herifli olmalidir "), MaxLength(32)]
        public string FullName { get; set; }
        public string Designation { get; set; }
        public IFormFile Image { get; set; }
        public int DepartmentId { get; set; }
    }
}
