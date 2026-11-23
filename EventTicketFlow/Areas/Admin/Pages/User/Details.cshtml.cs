using EventTicketFlow.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventTicketFlow.Areas.Admin.Pages.User;

public class DetailsModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    public DetailsModel(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public ApplicationUser UserDetails { get; set; } = default!;
    public IReadOnlyList<string> Permissions { get; private set; } = Array.Empty<string>();

    public async Task<IActionResult> OnGetAsync(string? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        UserDetails = user;
        Permissions = (await _userManager.GetRolesAsync(user))
            .OrderBy(role => role)
            .ToArray();
        return Page();
    }
}
