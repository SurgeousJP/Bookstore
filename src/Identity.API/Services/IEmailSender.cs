using Identity.API.Models;

namespace Identity.API.Services
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
        public Task SendEmailAsync(Message message);
    }
}
