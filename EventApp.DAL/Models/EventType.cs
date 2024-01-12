namespace EventApp.DAL.Models
{
    public class EventType
    {
        public int EventTypeId { get; set; }
        public string? EventTypeName { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime LastModeifed { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
}
