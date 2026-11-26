using System.Globalization;
using System.Text.Encodings.Web;
using EventTicketFlow.Models;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace EventTicketFlow.Services;

public class NotificationEmailService(IEmailSender sender, ILogger<NotificationEmailService> logger)
{
    public async Task SendBestEffortAsync(string email, string subject, string body)
    {
        try
        {
            await sender.SendEmailAsync(email, subject, body);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unable to send notification. Subject: {Subject}", subject);
        }
    }

    public Task SendPasswordChangedAsync(string email) => SendBestEffortAsync(
        email,
        "Your Event Ticket Flow password was changed",
        "<p>Your password was changed successfully.</p><p>If you did not make this change, reset your password immediately and contact support.</p>");

    public async Task SendPurchaseEmailsAsync(string email, Order order)
    {
        if (order.Id <= 0 || order.Tickets.Count == 0 || order.Tickets.Any(ticket =>
            ticket.Id <= 0 || ticket.Quantity <= 0 || ticket.TicketType?.Event == null))
        {
            throw new ArgumentException("A saved order with saved tickets and event details is required.", nameof(order));
        }

        var items = string.Join("", order.Tickets.Select(ticket =>
            $"<li>{HtmlEncoder.Default.Encode(ticket.TicketType!.Event!.EventName)}: {ticket.Quantity} ticket(s), {ticket.TotalCost.ToString("0.00", CultureInfo.InvariantCulture)}</li>"));
        await sender.SendEmailAsync(email, $"Purchase receipt #{order.Id}",
            $"<p>Thank you for your purchase.</p><p>Order #{order.Id}</p><ul>{items}</ul><p>Total: {order.total.ToString("0.00", CultureInfo.InvariantCulture)}</p>");

        var tickets = string.Join("", order.Tickets.Select(ticket =>
            $"<li>{HtmlEncoder.Default.Encode(ticket.TicketType!.Event!.EventName)}: {ticket.Quantity} ticket(s){(ticket.IsVip ? ", VIP" : "")}{(ticket.IsFastTrack ? ", Fast Track" : "")}</li>"));
        await sender.SendEmailAsync(email, $"Ticket confirmation for order #{order.Id}",
            $"<p>Your tickets are confirmed.</p><p>Order #{order.Id}</p><ul>{tickets}</ul><p>At the event, provide the name or email used for your purchase to the check-in staff.</p>");
    }
}