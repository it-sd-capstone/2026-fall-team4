using EventTicketFlow.Data;
using EventTicketFlow.Models;

namespace EventTicketFlow.Services
{
    public class EventService
    {
        AppDbContext _context;

        public EventService(AppDbContext context)
        {
            _context = context;
        }

        public List<Event> SearchEvents(string? searchTerm)
        {
            var events = _context.Events.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string term = searchTerm.Trim();

                events = events.Where(e => e.EventName.Contains(term) ||
                         e.EventDescription.Contains(term));
            }
            return events.OrderBy(e => e.OpenDate).ToList();
        }
    }
}
