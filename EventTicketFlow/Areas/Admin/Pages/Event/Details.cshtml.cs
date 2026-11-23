using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EventTicketFlow.Models;
using EventTicketFlow.Data;

namespace EventTicketFlow.Pages.EventPages;

public class DetailsModel : PageModel
{
    private readonly AppDbContext _context;

    public DetailsModel(AppDbContext context)
    {
        _context = context;
    }

    public Event Event { get; set; } = default!;
    public List<SalesDay> Sales { get; set; } = new();
    public int TicketsSold => Sales.Sum(day => day.Quantity);
    public int TicketsRemaining => Math.Max(0, Event.MaxTickets - TicketsSold);
    public decimal Revenue => Sales.Sum(day => day.Revenue);

    public record SalesDay(DateTime Date, int Quantity, decimal Revenue);

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
        var dailySales = await _context.PurchasedTickets.AsNoTracking()
            .Where(ticket => ticket.TicketType != null && ticket.TicketType.EventId == id)
            .GroupBy(ticket => ticket.CreatedDate.Date)
            .Select(group => new
            {
                Date = group.Key,
                Quantity = group.Sum(ticket => ticket.Quantity),
                Revenue = group.Sum(ticket => ticket.TotalCost)
            })
            .OrderBy(day => day.Date)
            .ToListAsync();
        Sales = dailySales
            .Select(day => new SalesDay(day.Date, day.Quantity, day.Revenue))
            .ToList();
        return Page();
    }
}