using Portfolio.Shared.Models;

namespace Portfolio.Api.Services;

public interface IEmailService
{
    Task SendContactNotificationAsync(ContactLead contactLead);
    Task SendContactConfirmationAsync(ContactLead contactLead);
}