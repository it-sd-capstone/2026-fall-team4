
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventTicketFlow.Models;
using EventTicketFlow.Data;

public class EventController : Controller
{
    private readonly AppDbContext _context;

    public EventController(AppDbContext context)
    {
        _context = context;
    }

    // GET: EVENTS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var eventItem = await _context.Events
            .FirstOrDefaultAsync(e => e.Id == id);

        if (eventItem == null)
        {
            return NotFound();
        }

        // Retrieve ticket pricing information associated with event
        var ticketType = await _context.TicketTypes
            .SingleOrDefaultAsync(t => t.EventId == id);

        // Pass ticket pricing information from database to view
        ViewBag.TicketType = ticketType;

        int ticketsSold = 0;

        if (ticketType != null)
        {

            // Collect number of tickets sold for each ticket type
            ticketsSold = await _context.PurchasedTickets
                .Where(p => p.TicketTypeId == ticketType.Id)
                .SumAsync(p => p.Quantity);
        }

        // Pass ticket sold information from database to view
        ViewBag.TicketsSold = ticketsSold;
        ViewBag.TicketsRemaining = eventItem.MaxTickets - ticketsSold;

        return View(eventItem);
    }


    
}
