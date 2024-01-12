namespace EventApp.DAL.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public string? EventName { get; set; }
        public byte[]? Image { get; set; }
        public DateTime? EventDate { get; set; }
        public string? Location { get; set;}
        public int? Limit { get; set; }
        public int TicketTypeId { get; set; }
        public TicketType? TicketType { get; set; }
        public int EventTypeId { get; set; }
        public EventType? EventType { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime LastModeifed { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
}
