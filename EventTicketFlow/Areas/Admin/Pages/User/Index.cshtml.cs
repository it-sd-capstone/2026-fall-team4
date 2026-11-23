using EventTicketFlow.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace EventTicketFlow.Areas.Admin.Pages.User;

public class IndexModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IndexModel(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public IList<ApplicationUser> Users { get; set; } = [];
    public string? Search { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

    public async Task OnGetAsync(string? search, int page = 1)
    {
        const int pageSize = 10;
        Search = search?.Trim();
        var users = _userManager.Users.AsNoTracking();
        var searchTerm = Search;
        if (!string.IsNullOrEmpty(searchTerm))
        {
            users = users.Where(user =>
                user.UserName!.Contains(searchTerm) ||
                user.Email!.Contains(searchTerm) ||
                (user.PhoneNumber != null && user.PhoneNumber.Contains(searchTerm)));
        }

        var count = await users.CountAsync();
        TotalPages = Math.Max(1, (int)Math.Ceiling(count / (double)pageSize));
        CurrentPage = Math.Clamp(page, 1, TotalPages);
        Users = await users.OrderBy(user => user.UserName).ThenBy(user => user.Id)
            .Skip((CurrentPage - 1) * pageSize).Take(pageSize).ToListAsync();
    }
}
