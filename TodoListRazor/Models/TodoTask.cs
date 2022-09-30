namespace TodoListRazor.Models
{
    public enum Importance
    {
        p1,
        p2,
        p3,
        p4
    }

    public class TodoTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DeadLine { get; set; }
        public bool IsCompleted { get; set; }
        public string CreatorId { get; set; }
        public Importance Importance { get; set; }
    }
}
