using EventTicketFlow.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventTicketFlow.Areas.Admin.Pages.User;

public class DeleteModel : PageModel
{
    private const string SelfDeleteMessage = "You cannot delete the account you are currently signed in with.";
    private const string LastAdminDeleteMessage = "The last user with the Admin permission cannot be deleted.";
    private const string AdminRole = "Admin";
    private readonly UserManager<ApplicationUser> _userManager;

    public DeleteModel(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [BindProperty]
    public string Id { get; set; } = string.Empty;

    public ApplicationUser UserDetails { get; set; } = default!;
    public bool IsCurrentUser { get; set; }
    public bool IsLastAdmin { get; set; }

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

        Id = user.Id;
        UserDetails = user;
        IsCurrentUser = IsSignedInUser(user);
        IsLastAdmin = await IsLastAdminAsync(user);
        if (IsCurrentUser)
        {
            ModelState.AddModelError(string.Empty, SelfDeleteMessage);
        }
        else if (IsLastAdmin)
        {
            ModelState.AddModelError(string.Empty, LastAdminDeleteMessage);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.FindByIdAsync(Id);
        if (user is null)
        {
            return NotFound();
        }

        UserDetails = user;
        IsCurrentUser = IsSignedInUser(user);
        IsLastAdmin = await IsLastAdminAsync(user);
        if (IsCurrentUser)
        {
            ModelState.AddModelError(string.Empty, SelfDeleteMessage);
            return Page();
        }

        if (IsLastAdmin)
        {
            ModelState.AddModelError(string.Empty, LastAdminDeleteMessage);
            return Page();
        }

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }

        return RedirectToPage("./Index");
    }

    private bool IsSignedInUser(ApplicationUser user)
    {
        var signedInUserId = _userManager.GetUserId(User);
        return signedInUserId is not null &&
               string.Equals(signedInUserId, user.Id, StringComparison.Ordinal);
    }

    private async Task<bool> IsLastAdminAsync(ApplicationUser user)
    {
        if (!await _userManager.IsInRoleAsync(user, AdminRole))
        {
            return false;
        }

        var admins = await _userManager.GetUsersInRoleAsync(AdminRole);
        return admins.Count <= 1;
    }
}
