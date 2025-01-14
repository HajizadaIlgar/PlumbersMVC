using PlumberzMVC.Models.Common;

namespace PlumberzMVC.Models
{
    public class Worker : BaseEntity
    {
        public string FullName { get; set; }
        public string Designation { get; set; }
        public string ImageUrl { get; set; }
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}
