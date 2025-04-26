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
        private readonly IPassengerRepository _passengerRepository;
        private readonly Func<NotificationChannel, INotificationFactory> _factoryResolver;
        private readonly IBookingRepository _bookingRepository;
        private IMapper _mapper;
        private readonly IContactRepository _contactRepository;

        public NotificationService(INotificationRepository notificationRepository,
            Func<NotificationChannel, INotificationFactory> factoryResolver,
            IContactRepository contactRepository,
            IMapper mapper,IBookingRepository bookingRepository, IPassengerRepository passengerRepository)
        {
            _notificationRepository = notificationRepository;
            _factoryResolver = factoryResolver;
            _mapper = mapper;
            _bookingRepository = bookingRepository;
            _contactRepository = contactRepository;
            _passengerRepository = passengerRepository;
        }
        public async Task<IEnumerable<GetNotificationDto>> GetAllInAppNotifications()
        {
            var notifications = await _notificationRepository.GetAllAsync();
            return  _mapper.Map<IEnumerable<GetNotificationDto>>(notifications);
        }

        public async Task<IEnumerable<GetNotificationDto>?> GetNotificationsByPassengerEmailAsync(string email)
        {
            var contacts = await _contactRepository.FindAsync(x => x.Email == email); 
            var foundContact = contacts.FirstOrDefault();
            if (foundContact == null) return null;

            var passenger = await _passengerRepository.FindAsync(x => x.ContactDetailsId == foundContact.Id);
            if (passenger == null) return null;

            var notifications = await _notificationRepository.FindAsync(x => x.PassengerId == passenger.Id);
            if (notifications == null) return null;


            return _mapper.Map<IEnumerable<GetNotificationDto>>(notifications);

        }
        public async Task<GeneralReponse> UpdateAsync(MarkNotificationReadDto mark,int notificationId)
        {
            var notification = _mapper.Map<Notification>(mark);
            notification.Id = notificationId;

            return await _notificationRepository.UpdateAsync(notification);
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
