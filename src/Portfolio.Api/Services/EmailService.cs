using System.Net;
using System.Net.Mail;
using Portfolio.Shared.Models;
using Microsoft.Extensions.Options;

namespace Portfolio.Api.Services;

public class EmailSettings
{
    public string SmtpServer { get; set; } = string.Empty;
    public int SmtpPort { get; set; }
    public string SmtpUsername { get; set; } = string.Empty;
    public string SmtpPassword { get; set; } = string.Empty;
    public string FromEmail { get; set; } = string.Empty;
    public string FromName { get; set; } = string.Empty;
    public string ToEmail { get; set; } = string.Empty;
    public bool EnableSsl { get; set; } = true;
}

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
    {
        _emailSettings = emailSettings.Value;
        _logger = logger;
    }

    public async Task SendContactNotificationAsync(ContactLead contactLead)
    {
        try
        {
            var subject = $"New Contact Form Submission - {contactLead.InterestType}";
            var body = $@"
<html>
<body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
    <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
        <h2 style='color: #667eea; border-bottom: 2px solid #667eea; padding-bottom: 10px;'>
            New Contact Form Submission
        </h2>
        
        <div style='background: #f8f9fa; padding: 20px; border-radius: 8px; margin: 20px 0;'>
            <h3 style='margin-top: 0; color: #495057;'>Contact Details</h3>
            <p><strong>Name:</strong> {contactLead.Name}</p>
            <p><strong>Email:</strong> <a href='mailto:{contactLead.Email}'>{contactLead.Email}</a></p>
            <p><strong>Company:</strong> {contactLead.Company ?? "Not specified"}</p>
            <p><strong>Interest Type:</strong> {contactLead.InterestType}</p>
            <p><strong>Submitted:</strong> {contactLead.CreatedAt:yyyy-MM-dd HH:mm:ss} UTC</p>
        </div>
        
        <div style='background: #fff; border: 1px solid #dee2e6; padding: 20px; border-radius: 8px;'>
            <h3 style='margin-top: 0; color: #495057;'>Message</h3>
            <p style='white-space: pre-wrap; margin: 0;'>{contactLead.Message}</p>
        </div>
        
        <div style='margin-top: 20px; padding: 15px; background: #e7f3ff; border-radius: 8px;'>
            <p style='margin: 0; font-size: 14px; color: #0066cc;'>
                <strong>Quick Actions:</strong> Reply directly to this email to respond to {contactLead.Name}.
            </p>
        </div>
    </div>
</body>
</html>";

            await SendEmailAsync(_emailSettings.ToEmail, subject, body, contactLead.Email);
            _logger.LogInformation("Contact notification email sent successfully for {Name} ({Email})", 
                contactLead.Name, contactLead.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send contact notification email for {Name} ({Email})", 
                contactLead.Name, contactLead.Email);
            throw;
        }
    }

    public async Task SendContactConfirmationAsync(ContactLead contactLead)
    {
        try
        {
            var subject = "Thank you for contacting me - Bernard Lusale";
            var body = $@"
<html>
<body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
    <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
        <h2 style='color: #667eea; border-bottom: 2px solid #667eea; padding-bottom: 10px;'>
            Thank You for Getting in Touch!
        </h2>
        
        <p>Hi {contactLead.Name},</p>
        
        <p>Thank you for reaching out through my portfolio website. I've received your message regarding <strong>{contactLead.InterestType.ToLower()}</strong> and I appreciate your interest.</p>
        
        <div style='background: #f8f9fa; padding: 20px; border-radius: 8px; margin: 20px 0;'>
            <h3 style='margin-top: 0; color: #495057;'>Your Message Summary</h3>
            <p><strong>Interest:</strong> {contactLead.InterestType}</p>
            <p><strong>Company:</strong> {contactLead.Company ?? "Not specified"}</p>
            <p><strong>Submitted:</strong> {contactLead.CreatedAt:yyyy-MM-dd HH:mm:ss} UTC</p>
        </div>
        
        <p>I'll review your message and get back to you within 24-48 hours. In the meantime, feel free to:</p>
        
        <ul>
            <li>Check out my <a href='https://github.com/Dev-Lusale' style='color: #667eea;'>GitHub profile</a> for more of my work</li>
            <li>Connect with me on <a href='https://www.linkedin.com/in/bernard-lusale-631b9b31a' style='color: #667eea;'>LinkedIn</a></li>
            <li>Download my <a href='mailto:bernardlusale20@gmail.com?subject=CV Request' style='color: #667eea;'>CV</a> if you haven't already</li>
        </ul>
        
        <p>Looking forward to our conversation!</p>
        
        <div style='margin-top: 30px; padding: 20px; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); border-radius: 8px; color: white;'>
            <p style='margin: 0; font-weight: bold;'>Bernard Lusale</p>
            <p style='margin: 5px 0 0 0; opacity: 0.9;'>Full Stack Developer</p>
            <p style='margin: 5px 0 0 0; opacity: 0.9;'>
                <a href='mailto:bernardlusale20@gmail.com' style='color: white;'>bernardlusale20@gmail.com</a>
            </p>
        </div>
    </div>
</body>
</html>";

            await SendEmailAsync(contactLead.Email, subject, body);
            _logger.LogInformation("Contact confirmation email sent successfully to {Name} ({Email})", 
                contactLead.Name, contactLead.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send contact confirmation email to {Name} ({Email})", 
                contactLead.Name, contactLead.Email);
            // Don't throw here - confirmation email failure shouldn't break the contact submission
        }
    }

    private async Task SendEmailAsync(string toEmail, string subject, string body, string? replyTo = null)
    {
        using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort);
        client.EnableSsl = _emailSettings.EnableSsl;
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);

        using var message = new MailMessage();
        message.From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName);
        message.To.Add(toEmail);
        message.Subject = subject;
        message.Body = body;
        message.IsBodyHtml = true;

        if (!string.IsNullOrEmpty(replyTo))
        {
            message.ReplyToList.Add(replyTo);
        }

        await client.SendMailAsync(message);
    }
}