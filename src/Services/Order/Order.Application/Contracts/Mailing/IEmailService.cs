namespace Order.Application.Contracts.Mailing;

public interface IEmailService
{
    Task SendEmail(Email email);
}
