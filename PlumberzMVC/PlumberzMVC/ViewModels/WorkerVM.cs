namespace PlumberzMVC.ViewModels
{
    public class WorkerVM
    {
        public string FullName { get; set; }
        public string Designation { get; set; }
        public IFormFile Image { get; set; }
        public int DepartmentId { get; set; }
    }
}
