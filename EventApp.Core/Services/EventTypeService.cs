using EventApp.DAL.Data;
using EventApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace EventApp.Core.Services
{
    public class EventTypeService : IEventTypeService
    {
        private readonly EventAppDbContext _context;

        public EventTypeService(EventAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EventType>> GetEventTypesAsync()
        {
            return await _context.EventTypes.Where(t => t.IsDeleted == false).ToListAsync();
        }

        public async Task<EventType> GetEventTypeByIdAsync(int id)
        {
            return await _context.EventTypes.FindAsync(id);
        }

        public async Task<EventType> CreateEventTypeAsync(EventType eventType)
        {
            _context.EventTypes.Add(eventType);
            await _context.SaveChangesAsync();
            return eventType;
        }

        public async Task UpdateEventTypeAsync(EventType eventType)
        {
            _context.Entry(eventType).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEventTypeAsync(int id)
        {
            var eventType = await _context.EventTypes.FindAsync(id);
            if (eventType != null)
            {
                // Set IsDeleted to true
                eventType.IsDeleted = true;

                // Update the TicketType in the context
                _context.EventTypes.Update(eventType);

                // Save changes to the database
                await _context.SaveChangesAsync();
            }
        }

    }
}
