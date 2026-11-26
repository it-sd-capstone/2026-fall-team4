using Microsoft.AspNetCore.Mvc;

namespace EventTicketFlow.Services;

public static class EmailLinkExtensions
{
    public static string EmailPage(this IUrlHelper url, string page, object values)
    {
        var services = url.ActionContext.HttpContext.RequestServices;
        var configuredUrl = services.GetRequiredService<IConfiguration>()["PUBLIC_BASE_URL"];
        var environment = services.GetRequiredService<IWebHostEnvironment>();
        if (!Uri.TryCreate(configuredUrl, UriKind.Absolute, out var origin) ||
            !string.IsNullOrEmpty(origin.UserInfo) || origin.AbsolutePath != "/" ||
            !string.IsNullOrEmpty(origin.Query) || !string.IsNullOrEmpty(origin.Fragment) ||
            (origin.Scheme != Uri.UriSchemeHttps &&
             !(environment.IsDevelopment() && origin.Scheme == Uri.UriSchemeHttp && origin.IsLoopback)))
        {
            throw new InvalidOperationException("PUBLIC_BASE_URL must be an HTTPS origin (HTTP localhost is allowed in development).");
        }

        var path = url.Page(page, pageHandler: null, values: values)
            ?? throw new InvalidOperationException("Could not generate an email link.");
        return new Uri(origin, path).AbsoluteUri;
    }
}