using System.ComponentModel.DataAnnotations;
using EventTicketFlow.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventTicketFlow.Areas.Admin.Pages.User;

public class EditModel : PageModel
{
    private const string AdminRole = "Admin";
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public EditModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public IReadOnlyList<string> AvailableRoles { get; private set; } = Array.Empty<string>();

    public class InputModel
    {
        [Required]
        public string Id { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Permissions")]
        public List<string> SelectedRoles { get; set; } = new();
    }

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

        await LoadAvailableRolesAsync();
        Input = new InputModel
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber,
            SelectedRoles = (await _userManager.GetRolesAsync(user)).ToList()
        };
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await LoadAvailableRolesAsync();
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _userManager.FindByIdAsync(Input.Id);
        if (user is null)
        {
            return NotFound();
        }

        Input.SelectedRoles ??= new List<string>();
        var selectedRoles = Input.SelectedRoles.Distinct(StringComparer.Ordinal).ToList();
        if (selectedRoles.Any(role => !AvailableRoles.Contains(role, StringComparer.Ordinal)))
        {
            ModelState.AddModelError(string.Empty, "One or more selected permissions are not valid.");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var currentRoles = await _userManager.GetRolesAsync(user);
        var isAdmin = currentRoles.Contains(AdminRole, StringComparer.OrdinalIgnoreCase);
        var willRemainAdmin = selectedRoles.Contains(AdminRole, StringComparer.OrdinalIgnoreCase);
        var admins = await _userManager.GetUsersInRoleAsync(AdminRole);
        var remainingAdminCount = admins.Count - (isAdmin && !willRemainAdmin ? 1 : 0);
        if (isAdmin && !willRemainAdmin)
        {
            if (string.Equals(user.Id, _userManager.GetUserId(User), StringComparison.Ordinal))
            {
                ModelState.AddModelError(string.Empty, "You cannot remove your own Admin permission.");
                return Page();
            }
        }

        if (remainingAdminCount <= 0 && !willRemainAdmin)
        {
            ModelState.AddModelError(string.Empty, "At least one user must retain the Admin permission.");
            return Page();
        }

        var emailChanged = !string.Equals(user.Email, Input.Email, StringComparison.OrdinalIgnoreCase);
        user.Email = Input.Email;
        user.UserName = Input.Email;
        user.PhoneNumber = Input.PhoneNumber;
        if (emailChanged)
        {
            user.EmailConfirmed = false;
        }

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }

        var rolesToRemove = currentRoles.Except(selectedRoles, StringComparer.OrdinalIgnoreCase).ToArray();
        if (rolesToRemove.Length > 0)
        {
            var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            if (!removeResult.Succeeded)
            {
                AddIdentityErrors(removeResult);
                return Page();
            }
        }

        var rolesToAdd = selectedRoles.Except(currentRoles, StringComparer.OrdinalIgnoreCase).ToArray();
        if (rolesToAdd.Length > 0)
        {
            var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
            if (!addResult.Succeeded)
            {
                AddIdentityErrors(addResult);
                return Page();
            }
        }

        return RedirectToPage("./Index");
    }

    private Task LoadAvailableRolesAsync()
    {
        AvailableRoles = _roleManager.Roles
            .Where(role => role.Name != null)
            .Select(role => role.Name!)
            .OrderBy(role => role)
            .ToList();
        return Task.CompletedTask;
    }

    private void AddIdentityErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }
}
