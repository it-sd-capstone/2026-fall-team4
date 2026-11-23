using EventTicketFlow.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EventTicketFlow.Pages.EventPages;

public class CheckInModel : PageModel
{
    private readonly AppDbContext _context;

    public CheckInModel(AppDbContext context)
    {
        _context = context;
    }

    public int EventId { get; set; }
    public string EventName { get; set; } = "";
    public string? Search { get; set; }
    public string Status { get; set; } = "all";
    public int TicketsSold { get; set; }
    public int CheckedIn { get; set; }
    public List<TicketRow> Tickets { get; set; } = new();

    public record TicketRow(int Id, string Name, string Email, int Quantity, bool CheckedIn);

    public async Task<IActionResult> OnGetAsync(int? id, string? search, string? status)
    {
        var eventEntity = await _context.Events.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id);
        if (eventEntity is null)
        {
            return NotFound();
        }

        EventId = eventEntity.Id;
        EventName = eventEntity.EventName;
        Search = search?.Trim();
        Status = status is "not" or "checked-in" ? status : "all";

        var purchases = _context.PurchasedTickets.AsNoTracking()
            .Where(ticket => ticket.TicketType != null && ticket.TicketType.EventId == id);
        TicketsSold = await purchases.SumAsync(ticket => (int?)ticket.Quantity) ?? 0;
        CheckedIn = await purchases.Where(ticket => ticket.CheckInDate != null)
            .SumAsync(ticket => (int?)ticket.Quantity) ?? 0;

        if (!string.IsNullOrEmpty(Search))
        {
            purchases = purchases.Where(ticket => ticket.Customer != null &&
                ((ticket.Customer.FirstName + " " + ticket.Customer.LastName).Contains(Search) ||
                 ticket.Customer.Email.Contains(Search)));
        }
        if (Status == "not")
        {
            purchases = purchases.Where(ticket => ticket.CheckInDate == null);
        }
        else if (Status == "checked-in")
        {
            purchases = purchases.Where(ticket => ticket.CheckInDate != null);
        }

        Tickets = await purchases.Where(ticket => ticket.Customer != null)
            .OrderBy(ticket => ticket.Customer!.LastName).ThenBy(ticket => ticket.Customer!.FirstName)
            .Select(ticket => new TicketRow(ticket.Id, ticket.Customer!.FirstName + " " + ticket.Customer.LastName,
                ticket.Customer.Email, ticket.Quantity, ticket.CheckInDate != null))
            .ToListAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, int ticketId, bool checkIn, string? search, string? status)
    {
        var purchases = _context.PurchasedTickets.Where(ticket => ticket.Id == ticketId &&
            ticket.TicketType != null && ticket.TicketType.EventId == id &&
            (checkIn ? ticket.CheckInDate == null : ticket.CheckInDate != null));
        var checkInDate = checkIn ? DateTime.UtcNow : (DateTime?)null;
        var updated = await purchases.ExecuteUpdateAsync(setters => setters.SetProperty(ticket => ticket.CheckInDate, checkInDate));

        TempData["CheckInMessage"] = updated == 0
            ? "The ticket was not found or its status has already changed."
            : checkIn ? "Tickets checked in." : "Tickets checked out.";
        return RedirectToPage(new { id, search, status });
    }
}