using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EventTicketFlow.Models;
using EventTicketFlow.Data;

namespace EventTicketFlow.Pages.EventPages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;

    public IndexModel(AppDbContext context)
    {
        _context = context;
    }

    public IList<Event> Event { get; set; } = default!;

    public string? Search { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

    public async Task OnGetAsync(string? search, int page = 1)
    {
        const int pageSize = 10;
        Search = search?.Trim();
        var events = _context.Events.AsNoTracking().AsQueryable();
        if (!string.IsNullOrEmpty(Search))
        {
            events = events.Where(item => item.EventName.Contains(Search));
        }

        var count = await events.CountAsync();
        TotalPages = Math.Max(1, (int)Math.Ceiling(count / (double)pageSize));
        CurrentPage = Math.Clamp(page, 1, TotalPages);
        Event = await events.OrderBy(item => item.EventName).ThenBy(item => item.Id)
            .Skip((CurrentPage - 1) * pageSize).Take(pageSize).ToListAsync();
    }
}
