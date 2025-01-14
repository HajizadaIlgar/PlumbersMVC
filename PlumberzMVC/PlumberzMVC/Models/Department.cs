using PlumberzMVC.Models.Common;

namespace PlumberzMVC.Models
{
    public class Department : BaseEntity
    {
        public string DepartmentName { get; set; }
        public ICollection<Worker> Workers { get; set; }
    }
}
