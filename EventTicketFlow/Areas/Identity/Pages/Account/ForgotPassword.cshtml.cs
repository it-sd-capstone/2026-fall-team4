// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using EventTicketFlow.Data;
using EventTicketFlow.Services;

namespace EventTicketFlow.Areas.Identity.Pages.Account;

public class ForgotPasswordModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly NotificationEmailService _notifications;
    private readonly ILogger<ForgotPasswordModel> _logger;

    public ForgotPasswordModel(UserManager<ApplicationUser> userManager, NotificationEmailService notifications, ILogger<ForgotPasswordModel> logger)
    {
        _userManager = userManager;
        _notifications = notifications;
        _logger = logger;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; } = default!;

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            // For more information on how to enable account confirmation and password reset please
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            try
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.EmailPage(
                    "/Account/ResetPassword",
                    new { area = "Identity", code });

                var storedEmail = await _userManager.GetEmailAsync(user);
                if (string.IsNullOrEmpty(storedEmail))
                {
                    throw new InvalidOperationException("Users must have an email.");
                }

                await _notifications.SendBestEffortAsync(
                    storedEmail,
                    "Reset your Event Ticket Flow password",
                    $"<p>We received a request to reset your password.</p><p><a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Reset your password</a></p><p>If you did not request this, you can ignore this email.</p>");
            }
            catch (InvalidOperationException exception)
            {
                _logger.LogError(exception, "Unable to create the password reset link.");
            }

            return RedirectToPage("./ForgotPasswordConfirmation");
        }

        return Page();
    }
}
