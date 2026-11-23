using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EventTicketFlow.Models;
using EventTicketFlow.Data;

namespace EventTicketFlow.Pages.EventPages;

public class EditModel : PageModel
{
    private readonly AppDbContext _context;

    public EditModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Event Event { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var eventEntity = await _context.Events.FirstOrDefaultAsync(m => m.Id == id);
        if (eventEntity is null)
        {
            return NotFound();
        }

        Event = eventEntity;
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Remove("Event.CreatedBy");
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var existingEvent = await _context.Events.FindAsync(Event.Id);
        if (existingEvent is null)
        {
            return NotFound();
        }

        existingEvent.EventName = Event.EventName;
        existingEvent.EventDescription = Event.EventDescription;
        existingEvent.EventCreated = Event.EventCreated;
        existingEvent.OpenDate = Event.OpenDate;
        existingEvent.CloseDate = Event.CloseDate;
        existingEvent.MaxTickets = Event.MaxTickets;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EventExists(Event.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool EventExists(int id)
    {
        return _context.Events.Any(e => e.Id == id);
    }
}