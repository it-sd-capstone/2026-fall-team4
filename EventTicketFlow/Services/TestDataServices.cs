using EventTicketFlow.Data;
using EventTicketFlow.Models;

namespace EventTicketFlow.Services
{
    public class TestDataService
    {
        AppDbContext _context;
        public TestDataService(AppDbContext context)
        {
            _context = context;
        }

        // Two shows with their prices. That is all the site needs to show events and prices.
        public void PopulateTestData()
        {
            // Only fill an empty database
            if (_context.Events.Any())
            {
                return;
            }

            // ----- Test Data 1 -----

            Event event1 = new Event();
            event1.CreatedBy = "admin";
            event1.EventName = "Winter Lights Festival";
            event1.EventDescription = "Live music, food stalls, and a lantern walk.";
            event1.EventCreated = new DateTime(2026, 11, 1);
            event1.OpenDate = new DateTime(2026, 11, 1);
            event1.CloseDate = new DateTime(2026, 12, 12);
            event1.MaxTickets = 250;

            TicketType ticketType1 = new TicketType();
            ticketType1.Event = event1;
            ticketType1.GeneralPrice = 25.00m;
            ticketType1.VipPrice = 40.00m;
            ticketType1.FastTrackPrice = 10.00m;

            // ----- Test Data 2 -----

            Event event2 = new Event();
            event2.CreatedBy = "admin";
            event2.EventName = "Comedy Night";
            event2.EventDescription = "An evening of stand-up comedy.";
            event2.EventCreated = new DateTime(2026, 11, 1);
            event2.OpenDate = new DateTime(2026, 11, 1);
            event2.CloseDate = new DateTime(2027, 2, 6);
            event2.MaxTickets = 100;

            TicketType ticketType2 = new TicketType();
            ticketType2.Event = event2;
            ticketType2.GeneralPrice = 15.00m;
            ticketType2.VipPrice = 30.00m;
            ticketType2.FastTrackPrice = 5.00m;

            _context.Events.Add(event1);
            _context.Events.Add(event2);
            _context.TicketTypes.Add(ticketType1);
            _context.TicketTypes.Add(ticketType2);

            _context.SaveChanges();
        }

    }

}