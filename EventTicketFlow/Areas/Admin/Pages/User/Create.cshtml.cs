using System.ComponentModel.DataAnnotations;
using EventTicketFlow.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventTicketFlow.Areas.Admin.Pages.User;

public class CreateModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public CreateModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
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
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [Display(Name = "Phone number")]
        public string? PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Display(Name = "Permissions")]
        public List<string> SelectedRoles { get; set; } = new();
    }

    public async Task OnGetAsync()
    {
        await LoadAvailableRolesAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await LoadAvailableRolesAsync();
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

        var user = new ApplicationUser
        {
            UserName = Input.Email,
            Email = Input.Email,
            PhoneNumber = Input.PhoneNumber
        };

        var createResult = await _userManager.CreateAsync(user, Input.Password);
        if (!createResult.Succeeded)
        {
            AddIdentityErrors(createResult);
            return Page();
        }

        if (selectedRoles.Count > 0)
        {
            var roleResult = await _userManager.AddToRolesAsync(user, selectedRoles);
            if (!roleResult.Succeeded)
            {
                AddIdentityErrors(roleResult);
                var cleanupResult = await _userManager.DeleteAsync(user);
                if (!cleanupResult.Succeeded)
                {
                    AddIdentityErrors(cleanupResult);
                    ModelState.AddModelError(
                        string.Empty,
                        $"The user account was created, but role assignment failed and the account could not be removed (user ID: {user.Id}). The account may have incomplete permissions.");
                }
                else
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "The user could not be created because permission assignment failed. The incomplete account was removed.");
                }

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
