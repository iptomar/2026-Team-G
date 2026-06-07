using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace _2026_Team_G.Services;

public class EmailSettings
{
    public string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public string SenderEmail { get; set; }
    public string SenderPassword { get; set; }
    public bool EnableSsl { get; set; }
}

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _emailSettings;

    public EmailSender(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // Se as configurações de email estiverem vazias (desenvolvimento local), simulamos o envio
        if (string.IsNullOrWhiteSpace(_emailSettings.SenderEmail) || string.IsNullOrWhiteSpace(_emailSettings.SmtpServer))
        {
            System.Diagnostics.Debug.WriteLine("========================================");
            System.Diagnostics.Debug.WriteLine($"[EMAIL SIMULADO] Para: {email}");
            System.Diagnostics.Debug.WriteLine($"[EMAIL SIMULADO] Assunto: {subject}");
            System.Diagnostics.Debug.WriteLine($"[EMAIL SIMULADO] Conteúdo: {htmlMessage}");
            System.Diagnostics.Debug.WriteLine("========================================");
            return;
        }

        using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
        {
            Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword),
            EnableSsl = _emailSettings.EnableSsl
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_emailSettings.SenderEmail),
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true
        };
        mailMessage.To.Add(email);

        await client.SendMailAsync(mailMessage);
    }
}