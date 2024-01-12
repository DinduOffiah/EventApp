using EventApp.DAL.Data;
using EventApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace EventApp.Core.Services
{
    public class TicketTypeService : ITicketTypeService
    {
        private readonly EventAppDbContext _context;

        public TicketTypeService(EventAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TicketType>> GetTicketTypesAsync()
        {
            return await _context.TicketTypes.Where(t => t.IsDeleted == false).ToListAsync();
        }

        public async Task<TicketType> GetTicketTypeByIdAsync(int id)
        {
            return await _context.TicketTypes.FindAsync(id);
        }

        public async Task<TicketType> CreateTicketTypeAsync(TicketType ticketType)
        {
            _context.TicketTypes.Add(ticketType);
            await _context.SaveChangesAsync();
            return ticketType;
        }

        public async Task UpdateTicketTypeAsync(TicketType ticketType)
        {
            _context.Entry(ticketType).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTicketTypeAsync(int id)
        {
            var ticketType = await _context.TicketTypes.FindAsync(id);
            if (ticketType != null)
            {
                // Set IsDeleted to true
                ticketType.IsDeleted = true;

                // Update the TicketType in the context
                _context.TicketTypes.Update(ticketType);

                // Save changes to the database
                await _context.SaveChangesAsync();
            }
        }

    }
}
