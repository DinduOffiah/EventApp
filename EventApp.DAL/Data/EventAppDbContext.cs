using EventApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace EventApp.DAL.Data
{
    public class EventAppDbContext : DbContext
    {
        public EventAppDbContext(DbContextOptions<EventAppDbContext> options)
          : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }


    }
}
