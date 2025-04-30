using AutoMapper;
using BaseEntity.Dtos;
using ServerLibrary.Command;
using ServerLibrary.Memento;
using ServerLibrary.Repositories.Interfaces;
using ServerLibrary.Services.Interfaces;
using ServerLibrary.Validators;

namespace ServerLibrary.FactoryMethod
{
    public class BookingCommandFactory : IBookingCommandFactory
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

        public BookingCommandFactory(
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

        public ValidateBookingDraftDetailsCommand CreateBookingDraftDetailsCommand(BookingDraftMemento draft)
            => new(_contactValidator, _passengerValidator,_passportValidator,draft);

        public CreateBookingCommand CreateCreateBookingCommand(CreateBookingDto createBooking, ProcessPassengerCommand proccessPassangerCmd, BookingDraftMemento draft)
            => new(_mapper,_bookingRepo,createBooking,proccessPassangerCmd,draft);

        public GenerateTicketsCommand CreateGenerateTicketsCommand(CreateBookingCommand createBookingCmd)
            => new(_ticketService, createBookingCmd);

        public ProcessPassengerCommand CreateProcessPassangerCommand(BookingDraftMemento draft)
            => new(_mapper,_passengerRepo,_contactRepo,_passportRepo,draft);

        public UpdateSeatAvailabilityCommand CreateUpdateSeatCommand(ValidateItineraryCommand itineraryCmd, int passengerNumber)
            => new(_flightRepo,itineraryCmd,passengerNumber);

        public ValidateBookingDraftCommand CreateValidateBookingDraftCommand(BookingDraftMemento draft)
            => new(draft);

        public ValidateItineraryCommand CreateValidateItineraryCommand(int itineraryId)
           => new(_itineraryRepo,itineraryId);
    }
}
