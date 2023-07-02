using Core.Contracts.Services.EmailService;

namespace Infrastructure.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendMessageAsync(EmailRequest emailRequest);
    }
}
