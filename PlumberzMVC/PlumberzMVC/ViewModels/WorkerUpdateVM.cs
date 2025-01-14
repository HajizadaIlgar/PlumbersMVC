namespace PlumberzMVC.ViewModels;

public class WorkerUpdateVM
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Designation { get; set; }
    public string FileUrl { get; set; }
    public IFormFile Image { get; set; }
    public int DepartmentId { get; set; }
}
