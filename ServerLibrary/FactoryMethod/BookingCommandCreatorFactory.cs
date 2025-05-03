using AutoMapper;
using BaseEntity.Enums;
using ServerLibrary.Command;
using ServerLibrary.Repositories.Interfaces;
using ServerLibrary.Services.Interfaces;
using ServerLibrary.Validators;

namespace ServerLibrary.FactoryMethod
{
    public class BookingCommandCreatorFactory : IBookingCommandCreatorFactory
    {
        private readonly IItineraryRepository _itineraryRepo;
        private readonly IContactValidator _contactValidator;
        private readonly IPassengerValidator _passengerValidator;
        private readonly IPassportValidator _passportValidator;
        private readonly IMapper _mapper;
        private readonly IPassengerRepository _passengerRepo;
        private readonly IContactRepository _contactRepo;
        private readonly IPassportIdentityRepository _passportRepo;
        private readonly IFlightRepository _flightRepo;
        private readonly IBookingRepository _bookingRepo;
        private readonly ITicketService _ticketService;

        public BookingCommandCreatorFactory(
                  IItineraryRepository itineraryRepo,
                  IContactValidator contactValidator,
                  IPassengerValidator passengerValidator,
                  IPassportValidator passportValidator,
                  IMapper mapper,
                  IPassengerRepository passengerRepo,
                  IContactRepository contactRepo,
                  IPassportIdentityRepository passportRepo,
                  IFlightRepository flightRepo,
                  IBookingRepository bookingRepo,
                  ITicketService ticketService)
        {
                _itineraryRepo = itineraryRepo;
                _contactValidator = contactValidator;
                _passengerValidator = passengerValidator;
                _passportValidator = passportValidator;
                _mapper = mapper;
                _passengerRepo = passengerRepo;
                _contactRepo = contactRepo;
                _passportRepo = passportRepo;
                _flightRepo = flightRepo;
                _bookingRepo = bookingRepo;
                _ticketService = ticketService;
        }

        public ICreateCommand<T> CreateBookingCommand<T>(BookingCommandType commandType) where T : IBookingCommand
        {
            return commandType switch
            {
                BookingCommandType.ValidateItinerary when typeof(T) == typeof(ValidateItineraryCommand)
                    => (ICreateCommand<T>)new ValidateItineraryCommandCreator(_itineraryRepo),

                BookingCommandType.ValidateDraft when typeof(T) == typeof(ValidateBookingDraftCommand)
                    => (ICreateCommand<T>)new ValidateBookingDraftCommandCreator(),

                BookingCommandType.ValidateSection when typeof(T) == typeof(ValidateBookingDraftDetailsCommand)
                    => (ICreateCommand<T>)new ValidateBookingDraftDetailsCreator(_contactValidator, _passengerValidator, _passportValidator),

                BookingCommandType.ProcessPassenger when typeof(T) == typeof(ProcessPassengerCommand)
                    => (ICreateCommand<T>)new ProcessPassengerCommandCreator(_mapper, _passengerRepo, _contactRepo, _passportRepo),

                BookingCommandType.UpdateSeat when typeof(T) == typeof(UpdateSeatAvailabilityCommand)
                    => (ICreateCommand<T>)new UpdateSeatAvailabilityCommandCreator(_flightRepo),

                BookingCommandType.CreateBooking when typeof(T) == typeof(CreateBookingCommand)
                    => (ICreateCommand<T>)new CreateBookingCommandCreator(_mapper, _bookingRepo),

                BookingCommandType.GenerateTickets when typeof(T) == typeof(GenerateTicketsCommand)
                    => (ICreateCommand<T>)new GenerateTicketCommandCreator(_ticketService),

                _ => throw new ArgumentException($"Invalid or mismatched command type: {commandType}")
            };
        }

    }
}
