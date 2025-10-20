using JoyModels.Models.DataTransferObjects.RequestTypes.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace JoyModels.Communications.Services;

public class EmailService(IConfiguration configuration) : IEmailService
{
    public async Task SendEmailAsync(EmailSendRequest request)
    {
        var emailServiceDetails = configuration.GetSection("Email");

        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(emailServiceDetails["Auth:Username"]));
        email.To.Add(MailboxAddress.Parse(request.To));
        email.Subject = request.Subject;
        email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(emailServiceDetails["Host"], int.Parse(emailServiceDetails["Port"]!),
            SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(emailServiceDetails["Auth:Username"], emailServiceDetails["Auth:Password"]);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}