using EventApp.DAL.Models;

namespace EventApp.Core.Services
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetEventAsync(string query);
        Task<Event> GetEventByIdAsync(int id);
        Task<Event> CreateEventAsync(Event events);
        Task UpdateEventAsync(Event events);
        Task DeleteEventAsync(int id);
    }
}
