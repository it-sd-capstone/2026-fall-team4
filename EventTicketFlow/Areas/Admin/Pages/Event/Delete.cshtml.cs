using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EventTicketFlow.Models;
using EventTicketFlow.Data;

namespace EventTicketFlow.Pages.EventPages;

public class DeleteModel : PageModel
{
    private readonly AppDbContext _context;

    public DeleteModel(AppDbContext context)
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

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var eventEntity = await _context.Events.FindAsync(id);
        if (eventEntity is not null)
        {
            Event = eventEntity;
            _context.Events.Remove(Event);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}