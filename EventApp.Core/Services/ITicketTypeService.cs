using EventApp.DAL.Models;

namespace EventApp.Core.Services
{
    public interface ITicketTypeService
    {
        Task<IEnumerable<TicketType>> GetTicketTypesAsync();
        Task<TicketType> GetTicketTypeByIdAsync(int id);
        Task<TicketType> CreateTicketTypeAsync(TicketType ticketType);
        Task UpdateTicketTypeAsync(TicketType ticketType);
        Task DeleteTicketTypeAsync(int id);
    }
}
