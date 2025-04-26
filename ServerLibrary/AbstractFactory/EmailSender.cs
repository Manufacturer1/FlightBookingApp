using BaseEntity.Configurations;
using BaseEntity.Entities;
using ServerLibrary.Repositories.Interfaces;
using System.Net;
using System.Net.Mail;

namespace ServerLibrary.AbstractFactory
{
    public class EmailSender : INotificationSender
    {
        private readonly IPassengerRepository _passengerRepository;

        public EmailSender(IPassengerRepository passengerRepository)
        {
            _passengerRepository = passengerRepository;
        }

        public async Task Send(Notification notification)
        {
            using var smtp = new SmtpClient(EmailConfiguration.Instance.Host)
            {
                Port = EmailConfiguration.Instance.Port,
                Credentials = new NetworkCredential(EmailConfiguration.Instance.Email, EmailConfiguration.Instance.Password),
                EnableSsl = true
            };

            var passenger = await _passengerRepository.GetAsync(notification.PassengerId);
            if (passenger == null) throw new ArgumentNullException("The passenger was not found.");
            var contactEmail = passenger.ContactDetails?.Email ?? "notfound@.mail.com";
            var emailMessage = new MailMessage
            {
                From = new MailAddress(EmailConfiguration.Instance.Email!),
                Subject = notification.Subject,
                Body = notification.Body,
                IsBodyHtml = true
            };

            emailMessage.To.Add(contactEmail);

            await smtp.SendMailAsync(emailMessage);
        }
    }
}
