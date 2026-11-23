
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

    // GET: EVENTS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Events.ToListAsync());
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

    // GET: EVENTS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: EVENTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,CreatedBy,EventName,EventDescription,EventCreated,OpenDate,CloseDate,MaxTickets")] Event eventItem)
    {
        if (ModelState.IsValid)
        {
            _context.Add(eventItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(eventItem);
    }

    // GET: EVENTS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var eventItem = await _context.Events.FindAsync(id);
        if (eventItem == null)
        {
            return NotFound();
        }
        return View(eventItem);
    }

    // POST: EVENTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,CreatedBy,EventName,EventDescription,EventCreated,OpenDate,CloseDate,MaxTickets")] Event eventItem)
    {
        if (id != eventItem.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(eventItem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(eventItem.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(eventItem);
    }

    // GET: EVENTS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var eventItem = await _context.Events
            .FirstOrDefaultAsync(m => m.Id == id);
        if (eventItem == null)
        {
            return NotFound();
        }

        return View(eventItem);
    }

    // POST: EVENTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var eventItem = await _context.Events.FindAsync(id);
        if (eventItem != null)
        {
            _context.Events.Remove(eventItem);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool EventExists(int? id)
    {
        return _context.Events.Any(e => e.Id == id);
    }
}
