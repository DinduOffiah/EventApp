using EventApp.DAL.Models;

namespace EventApp.Core.Services
{
    public interface IEventTypeService
    {
        Task<IEnumerable<EventType>> GetEventTypesAsync();
        Task<EventType> GetEventTypeByIdAsync(int id);
        Task<EventType> CreateEventTypeAsync(EventType eventType);
        Task UpdateEventTypeAsync(EventType eventType);
        Task DeleteEventTypeAsync(int id);
    }
}
