using AutoMapper;
using BaseEntity.Dtos;
using BaseEntity.Entities;
using BaseEntity.Enums;
using BaseEntity.Responses;
using ServerLibrary.AbstractFactory;
using ServerLibrary.Repositories.Interfaces;
using ServerLibrary.Services.Interfaces;

namespace ServerLibrary.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly Func<NotificationChannel, INotificationFactory> _factoryResolver;
        private readonly IBookingRepository _bookingRepository;
        private IMapper _mapper;

        public NotificationService(INotificationRepository notificationRepository,
            Func<NotificationChannel, INotificationFactory> factoryResolver,
            IMapper mapper,IBookingRepository bookingRepository)
        {
            _notificationRepository = notificationRepository;
            _factoryResolver = factoryResolver;
            _mapper = mapper;
            _bookingRepository = bookingRepository;
        }
        public async Task<IEnumerable<GetNotificationDto>> GetAllInAppNotifications()
        {
            var notifications = await _notificationRepository.GetAllAsync();
            return  _mapper.Map<IEnumerable<GetNotificationDto>>(notifications);
        }

        public async Task<GeneralReponse> SendBookingConfirmation(SendNotificationDto sendNotification)
        {
            string notificationSubject = "Booking Confirmation";

            var booking = await _bookingRepository.GetAsync(sendNotification.BookingId);

            if (booking == null) return new GeneralReponse(false, "Booking was not found.");

            await SendNotification(NotificationChannel.Email, notificationSubject, booking);
            await SendNotification(NotificationChannel.InApp, notificationSubject, booking);


            return new GeneralReponse(true, "Notifications was sent successfuly!");
        }

        private async Task SendNotification(NotificationChannel channel,string subject,Booking booking)
        {
            var factory = _factoryResolver(channel);
            var template = factory.CreateTemplateGenerator();
            var sender = factory.CreateSender();

            var notification = new Notification
            {
                Subject = subject,
                Body = template.GenerateBookingConfirmation(booking),
                PassengerId = booking.PassengerId,
                IsRead = false,
                CreatedAt = DateTime.Now,
                
            };
            await sender.Send(notification);
        }
    }
}
