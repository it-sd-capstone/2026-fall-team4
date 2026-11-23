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

        public List<Event> SearchEvents(string? searchTerm, string? status, string? price, string? sort)
        {
            var events = _context.Events.AsQueryable();

            DateTime now = DateTime.Now;


            // Filter for event status
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string term = searchTerm.Trim();

                events = events.Where(e => e.EventName.Contains(term) ||
                         e.EventDescription.Contains(term));
            }
            if (status == "open")
            {
                events = events.Where(e => e.OpenDate <= now && e.CloseDate >= now);
            }
            else if (status == "upcoming")
            {
                events = events.Where(e => e.OpenDate > now);
            }
            else if (status == "closed")
            {
                events = events.Where(e => e.CloseDate < now);
            }
            
            // Filter by price range
            if (price == "under25")
            {
                events = events.Where( e=> _context.TicketTypes.Any(t => t.EventId == 
                                       e.Id && t.GeneralPrice < 25));
            }
            else if (price == "25to75")
            {
                events = events.Where(e => _context.TicketTypes.Any(t => t.EventId ==
                                       e.Id && t.GeneralPrice >= 25 && t.GeneralPrice <= 75));
            }
            else if (price == "over75")
            {
                events = events.Where(e => _context.TicketTypes.Any(t => t.EventId ==
                                       e.Id && t.GeneralPrice > 75));
            }

            // Sort list desc or asc by price
            if (sort == "priceLowToHigh")
            {
                events = events.OrderBy(e => _context.TicketTypes
                    .Where(t => t.EventId == e.Id)
                    .Select(t => t.GeneralPrice).FirstOrDefault());
            }
            else if (sort == "priceHighToLow")
            {
                events = events.OrderByDescending(e => _context.TicketTypes
                    .Where(t => t.EventId == e.Id)
                    .Select(t => t.GeneralPrice).FirstOrDefault());
            }
            else if (sort == "name")
            {
                events = events.OrderBy(e => e.EventName);
            }
            else // sort by date starting with soonest
            {
                events = events.OrderBy(e => e.OpenDate);
            }

            return events.ToList();
        }
    }
}
