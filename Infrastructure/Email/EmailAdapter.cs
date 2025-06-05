
using System.Net;
using System.Net.Mail;

namespace Infrastructure;

public class EmailAdapter : IEmailSender
{
    private readonly string host;
    private readonly int port;
    private readonly bool enableSSL;
    private readonly string userName;
    private readonly string password;

    public EmailAdapter(string host, int port, bool enableSSL, string userName, string password)
    {
        this.host = host;
        this.port = port;
        this.enableSSL = enableSSL;
        this.userName = userName;
        this.password = password;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var client = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(userName, password),
            EnableSsl = enableSSL,
        };
        await client.SendMailAsync(
            new MailMessage(userName, email, subject, htmlMessage) { IsBodyHtml = true }
        );
    }
}