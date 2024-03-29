using EventApp.DAL.Data;
using EventApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace EventApp.Core.Services
{
    public class EventService : IEventService
    {
        private readonly EventAppDbContext _context;

        public EventService(EventAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Event>> GetEventAsync()
        {
            return await _context.Events
        .Where(e => e.IsDeleted == false)
        .Include(e => e.EventType)
        .Include(e => e.TicketType).ToListAsync();
        }

        public async Task<Event> GetEventByIdAsync(int id)
        {
            return await _context.Events.FindAsync(id);
        }

        public async Task<Event> CreateEventAsync(Event events)
        {
            _context.Events.Add(events);
            await _context.SaveChangesAsync();
            return events;
        }

        public async Task UpdateEventAsync(Event events)
        {
            _context.Entry(events).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEventAsync(int id)
        {
            var events = await _context.Events.FindAsync(id);
            if (events != null)
            {
                // Set IsDeleted to true
                events.IsDeleted = true;

                // Update the TicketType in the context
                _context.Events.Update(events);

                // Save changes to the database
                await _context.SaveChangesAsync();
            }
        }
    }
}
