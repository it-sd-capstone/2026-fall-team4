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

        // Test events to populate the events table in the Azure DB, we could reuse as real later if wanted. 
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
            event1.OpenDate = new DateTime(2026, 11, 12);
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
            event2.OpenDate = new DateTime(2026, 10, 12);
            event2.CloseDate = new DateTime(2026, 11, 12);
            event2.MaxTickets = 50;

            TicketType ticketType2 = new TicketType();
            ticketType2.Event = event2;
            ticketType2.GeneralPrice = 15.00m;
            ticketType2.VipPrice = 30.00m;
            ticketType2.FastTrackPrice = 5.00m;

            // ----- Test Data 3 -----

            Event event3 = new Event();
            event3.CreatedBy = "admin";
            event3.EventName = "Movie Night";
            event3.EventDescription = "A night to marathon movies with friends and families.";
            event3.EventCreated = new DateTime(2026, 11, 20);
            event3.OpenDate = new DateTime(2026, 11, 13);
            event3.CloseDate = new DateTime(2026, 12, 13);
            event3.MaxTickets = 115;

            TicketType ticketType3 = new TicketType();
            ticketType3.Event = event3;
            ticketType3.GeneralPrice = 5.00m;
            ticketType3.VipPrice = 20.00m;
            ticketType3.FastTrackPrice = 5.00m;

            // ----- Test Data 4 -----

            Event event4 = new Event();
            event4.CreatedBy = "admin";
            event4.EventName = "Music Festival";
            event4.EventDescription = "A day of live music performances.";
            event4.EventCreated = new DateTime(2026, 11, 20);
            event4.OpenDate = new DateTime(2027, 4, 1);
            event4.CloseDate = new DateTime(2027, 5, 1);
            event4.MaxTickets = 1000;

            TicketType ticketType4 = new TicketType();
            ticketType4.Event = event4;
            ticketType4.GeneralPrice = 110.00m;
            ticketType4.VipPrice = 125.00m;
            ticketType4.FastTrackPrice = 5.00m;

            // ----- Test Data 5 -----

            Event event5 = new Event();
            event5.CreatedBy = "admin";
            event5.EventName = "Professional Tortoise Race";
            event5.EventDescription = "A day of competitive tortoise racing.";
            event5.EventCreated = new DateTime(2026, 11, 20);
            event5.OpenDate = new DateTime(2027, 3, 19);
            event5.CloseDate = new DateTime(2027, 4, 19);
            event5.MaxTickets = 65;

            TicketType ticketType5 = new TicketType();
            ticketType5.Event = event5;
            ticketType5.GeneralPrice = 55.00m;
            ticketType5.VipPrice = 75.00m;
            ticketType5.FastTrackPrice = 5.00m;

            // ----- Test Data 6 -----

            Event event6 = new Event();
            event6.CreatedBy = "admin";
            event6.EventName = "Sandwich Eating Contest";
            event6.EventDescription = "Competitive sandwich eating. Are you hungry enough?";
            event6.EventCreated = new DateTime(2026, 11, 20);
            event6.OpenDate = new DateTime(2027, 2, 14);
            event6.CloseDate = new DateTime(2027, 3, 14);
            event6.MaxTickets = 20;

            TicketType ticketType6 = new TicketType();
            ticketType6.Event = event6;
            ticketType6.GeneralPrice = 10.00m;
            ticketType6.VipPrice = 25.00m;
            ticketType6.FastTrackPrice = 5.00m;

            // ----- Test Data 7 -----

            Event event7 = new Event();
            event7.CreatedBy = "admin";
            event7.EventName = "Eau Claire Card Show";
            event7.EventDescription = "A card show in Eau Claire! Come shop, trade, admire, and play!";
            event7.EventCreated = new DateTime(2026, 11, 20);
            event7.OpenDate = new DateTime(2026, 12, 9);
            event7.CloseDate = new DateTime(2027, 1, 9);
            event7.MaxTickets = 500;

            TicketType ticketType7 = new TicketType();
            ticketType7.Event = event7;
            ticketType7.GeneralPrice = 5.00m;
            ticketType7.VipPrice = 20.00m;
            ticketType7.FastTrackPrice = 10.00m;

            // ----- Test Data 8 -----

            Event event8 = new Event();
            event8.CreatedBy = "admin";
            event8.EventName = "Meet and Greet with a Mystery Celebrity";
            event8.EventDescription = "Come by for fun and a mystery guest that will be hosting a meet and greet!";
            event8.EventCreated = new DateTime(2026, 11, 20);
            event8.OpenDate = new DateTime(2027, 10, 31);
            event8.CloseDate = new DateTime(2027, 11, 30);
            event8.MaxTickets = 1500;

            TicketType ticketType8 = new TicketType();
            ticketType8.Event = event8;
            ticketType8.GeneralPrice = 80.00m;
            ticketType8.VipPrice = 95.00m;
            ticketType8.FastTrackPrice = 10.00m;

            // ----- Test Data 9 -----

            Event event9 = new Event();
            event9.CreatedBy = "admin";
            event9.EventName = "Tech Showdown!";
            event9.EventDescription = "Join the competition and put your tech knowledge to the test!";
            event9.EventCreated = new DateTime(2026, 11, 20);
            event9.OpenDate = new DateTime(2028, 3, 5);
            event9.CloseDate = new DateTime(2028, 4, 5);
            event9.MaxTickets = 275;

            TicketType ticketType9 = new TicketType();
            ticketType9.Event = event9;
            ticketType9.GeneralPrice = 44.99m;
            ticketType9.VipPrice = 59.99m;
            ticketType9.FastTrackPrice = 5.00m;

            // ----- Test Data 10 -----

            Event event10 = new Event();
            event10.CreatedBy = "admin";
            event10.EventName = "Coding Bootcamp";
            event10.EventDescription = "Intensive coding workshop for beginners.";
            event10.EventCreated = new DateTime(2026, 11, 20);
            event10.OpenDate = new DateTime(2028, 5, 1);
            event10.CloseDate = new DateTime(2028, 5, 31);
            event10.MaxTickets = 100;

            TicketType ticketType10 = new TicketType();
            ticketType10.Event = event10;
            ticketType10.GeneralPrice = 199.99m;
            ticketType10.VipPrice = 249.99m;
            ticketType10.FastTrackPrice = 25.00m;

            _context.Events.Add(event1);
            _context.Events.Add(event2);
            _context.Events.Add(event3);
            _context.Events.Add(event4);
            _context.Events.Add(event5);
            _context.Events.Add(event6);
            _context.Events.Add(event7);
            _context.Events.Add(event8);
            _context.Events.Add(event9);
            _context.Events.Add(event10);
            _context.TicketTypes.Add(ticketType1);
            _context.TicketTypes.Add(ticketType2);
            _context.TicketTypes.Add(ticketType3);
            _context.TicketTypes.Add(ticketType4);
            _context.TicketTypes.Add(ticketType5);
            _context.TicketTypes.Add(ticketType6);
            _context.TicketTypes.Add(ticketType7);
            _context.TicketTypes.Add(ticketType8);
            _context.TicketTypes.Add(ticketType9);
            _context.TicketTypes.Add(ticketType10);

            _context.SaveChanges();
        }

    }

}