using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Services.Interfaces;

namespace Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendAsync(string to, string subject, string body)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_config["Email:From"]));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html) 
        { 
            Text = body 
        };

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(
            _config["Email:Smtp"],
            int.Parse(_config["Email:Port"]),
            SecureSocketOptions.StartTls
        );
       
        await smtp.AuthenticateAsync(
            _config["Email:Username"],
            _config["Email:Password"]
        );

        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}