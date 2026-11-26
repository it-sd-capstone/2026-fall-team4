using System.Net;
using System.Text.RegularExpressions;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace EventTicketFlow.Services;

public class SmtpEmailSender(IConfiguration configuration, ILogger<SmtpEmailSender> logger) : IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var host = configuration["SMTP_HOST"];
        var username = configuration["SMTP_USERNAME"];
        var password = configuration["SMTP_PASSWORD"];
        var from = configuration["SMTP_FROM_EMAIL"];
        var portValue = configuration["SMTP_PORT"] ?? "587";

        if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(from) ||
            !int.TryParse(portValue, out var port) || port is < 1 or > 65535)
        {
            logger.LogError("SMTP configuration is missing or invalid.");
            throw new InvalidOperationException("Email delivery is not configured.");
        }

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(configuration["SMTP_FROM_NAME"] ?? "Event Ticket Flow", from));
        message.To.Add(MailboxAddress.Parse(email));
        message.Subject = subject;

        var plainText = Regex.Replace(htmlMessage, "<a\\s+[^>]*href=['\"]([^'\"]+)['\"][^>]*>(.*?)</a>", "$2 ($1)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        plainText = Regex.Replace(plainText, "<[^>]+>", " ");
        message.Body = new BodyBuilder
        {
            HtmlBody = htmlMessage,
            TextBody = WebUtility.HtmlDecode(plainText)
        }.ToMessageBody();

        try
        {
            using var client = new SmtpClient();
            await client.ConnectAsync(host, port, port == 465 ? SecureSocketOptions.SslOnConnect : port == 587 ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto);
            await client.AuthenticateAsync(username, password);
            await client.SendAsync(message);
            logger.LogInformation("Email accepted by SMTP server. Subject: {Subject}", subject);
            try
            {
                await client.DisconnectAsync(true);
            }
            catch (Exception exception)
            {
                logger.LogWarning(exception, "SMTP disconnect failed after the message was accepted.");
            }
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "SMTP delivery failed. Subject: {Subject}", subject);
            throw;
        }
    }
}