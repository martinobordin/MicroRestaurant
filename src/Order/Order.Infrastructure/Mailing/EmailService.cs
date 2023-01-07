using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Order.Application.Contracts.Mailing;

namespace Order.Infrastructure.Mailing
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;
        private readonly ILogger<EmailService> logger;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            this.emailSettings = emailSettings.Value;
            this.logger = logger;
        }

        public Task SendEmail(Email email)
        {
            this.logger.LogInformation("Sent email to: {To}, subject: {Subject}, body: {Body}", email.To, email.Subject, email.Body);
            return Task.CompletedTask;
        }
    }
}
