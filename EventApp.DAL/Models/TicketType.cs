namespace EventApp.DAL.Models
{
    public class TicketType
    {
        public int TicketTypeId { get; set; }
        public string? TicketTypeName { get; set;}
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime LastModeifed { get; set; }= DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
}
