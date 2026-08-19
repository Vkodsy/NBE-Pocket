using Microsoft.Extensions.Configuration;
using NBEProject1.Services;
using System.Net;
using System.Net.Mail;

namespace UserAuthApi.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendConfirmationEmailAsync(string toEmail, string confirmationLink)
    {
        var host = _configuration["EmailSettings:Host"];
        var port = int.Parse(_configuration["EmailSettings:Port"] ?? "587");
        var senderEmail = _configuration["EmailSettings:SenderEmail"];
        var senderPassword = _configuration["EmailSettings:Password"];
        var senderName = _configuration["EmailSettings:SenderDisplayName"] ?? "Support";

        using var client = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(senderEmail, senderPassword),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(senderEmail!, senderName),
            Subject = "Verify your account",
            Body = $"""
                <h2>Email Confirmation</h2>
                <p>Thank you for registering! Please confirm your email address by clicking the link below:</p>
                <p><a href="{confirmationLink}">Verify My Email</a></p>
                <p>This link will expire in 24 hours.</p>
                """,
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);

        await client.SendMailAsync(mailMessage);
    }
}