namespace PlumberzMVC.Models.Common
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
