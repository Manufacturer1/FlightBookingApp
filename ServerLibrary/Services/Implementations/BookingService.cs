using AutoMapper;
using BaseEntity.Dtos;
using BaseEntity.Entities;
using BaseEntity.Responses;
using Microsoft.AspNetCore.Http;
using ServerLibrary.Memento;
using ServerLibrary.Repositories.Interfaces;
using ServerLibrary.Services.Interfaces;

namespace ServerLibrary.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly IItineraryRepository _itineraryRepository;
        private IMapper _mapper;
        private readonly BookingWizardHistory _bookingHistory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPassengerRepository _passengerRepository;
        private readonly IPassportIdentityRepository _passportIdentityRepository;
        private readonly IContactRepository _contactRepository;
        private readonly ITicketService _ticketService;

        public BookingService(IBookingRepository bookingRepository,
            IFlightRepository flightRepository,
            IItineraryRepository itineraryRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IPassengerRepository passengerRepository,
            IPassportIdentityRepository passportIdentityRepository,
            IContactRepository contactRepository,
            ITicketService ticketService)
        {
            _bookingRepository = bookingRepository;
            _flightRepository = flightRepository;
            _mapper = mapper;
            _itineraryRepository = itineraryRepository;
            _httpContextAccessor = httpContextAccessor;
            _bookingHistory = new BookingWizardHistory(httpContextAccessor.HttpContext!.Session);
            _passengerRepository = passengerRepository;
            _passportIdentityRepository = passportIdentityRepository;
            _contactRepository = contactRepository;
            _ticketService = ticketService;
        }


        public async Task<BookingResponse> BookSeatAsync(CreateBookingDto createBooking)
        {
            // Step 1: Validate itinerary
            var itinerary = await _itineraryRepository.GetByIdAsync(createBooking.ItineraryId);
            if (itinerary == null)
                return new BookingResponse(false, "Itinerary does not exist.", null);

            // Step 2: Load current booking draft from Memento
            var draft = GetCurrentBookingState();
            if (draft == null)
                return new BookingResponse(false, "Booking history missing", null);

            // Step 3: Validate input sections
            if (draft.ContactDetails == null || draft.Passenger == null || draft.Passport == null)
                return new BookingResponse(false, "Incomplete booking draft", null);

            var contactValidation = ValidateContactDetails(draft.ContactDetails);
            if (!contactValidation.Flag) return new BookingResponse(contactValidation.Flag, contactValidation.Message, null);

            var passengerValidation = ValidatePassenger(draft.Passenger);
            if (!passengerValidation.Flag) return new BookingResponse(passengerValidation.Flag, passengerValidation.Message, null);

            var passportValidation = ValidatePassport(draft.Passport);
            if (!passportValidation.Flag) return new BookingResponse(passportValidation.Flag, passportValidation.Message, null);

            if (string.IsNullOrEmpty(draft.PaymentIntentId))
                return new BookingResponse(false, "Payment intent is null.", null);

            // Step 4: Map and persist entities
            try
            {
                var contact = _mapper.Map<ContactDetails>(draft.ContactDetails);
                var existingContacts = await _contactRepository.FindAsync(x => x.Email == draft.ContactDetails.Email);

                var existingContact = existingContacts.FirstOrDefault();
                int contactId = 0;

                if (existingContact != null)
                {
                    contact.Id = existingContact.Id;

                    var updateResponse = await _contactRepository.UpdateAsync(contact);
                    if (!updateResponse.Flag) return new BookingResponse(false, updateResponse.Message, null);

                    contactId = existingContact.Id;
                }
                else
                {
                    contactId = await _contactRepository.CreateAsync(contact);
                }

                var passport = _mapper.Map<PassportIdentity>(draft.Passport);
                var passportId = await _passportIdentityRepository.CreateAsync(passport);
       

                var passenger = _mapper.Map<Passenger>(draft.Passenger);
                var existingPassenger = 
                passenger.ContactDetailsId = contactId;
                passenger.PassportIdentityId = passportId;
                var passengerId = await _passengerRepository.CreateAsync(passenger);
           

                // Step 5: Create booking
                var booking = _mapper.Map<Booking>(createBooking);
                booking.PassengerId = passengerId;
                booking.PaymentIntentId = draft.PaymentIntentId;
                booking.BookingDate = DateTime.Now;

                // Step 6: Update seat availability
                foreach (var segment in itinerary.Segments!)
                {
                    var existingFlight = await _flightRepository.GetByFlightNumberAsync(segment.FlightNumber);
                    if (existingFlight == null)
                        return new BookingResponse(false, $"Flight {segment.FlightNumber} does not exist.", null);

                    if (existingFlight.AvailableSeats < createBooking.PassengerNumberSelected)
                        return new BookingResponse(false, $"Not enough seats on flight {segment.FlightNumber}.", null);

                    existingFlight.AvailableSeats -= createBooking.PassengerNumberSelected;
                    await _flightRepository.UpdateAsync(existingFlight);
                }

                // Step 7: Save booking
                var (createResult,bookingId) =  await _bookingRepository.CreateAsync(booking);
                if (!createResult.Flag || !bookingId.HasValue)
                    return new BookingResponse(createResult.Flag, createResult.Message, null);


                // Step 8: Generate tickets
                var createTicket = new CreateTicketDto { BookingId = bookingId.Value };
                var (ticketResponse,tickets) = await _ticketService.GenerateTicketAsync(createTicket);
                if (!ticketResponse.Flag || tickets == null && !tickets!.Any()) return new BookingResponse(ticketResponse.Flag, ticketResponse.Message, null);




                return new BookingResponse(true, "Successfuly booked and generated tickets", tickets);
            }
            catch(Exception ex)
            {
                return new BookingResponse(false, ex.Message, null);
            }
        }


        public async Task<IEnumerable<GetBookingDto>> GetAllBookingsAsync()
        {
            var bookings = await _bookingRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GetBookingDto>>(bookings);
        }

        public async Task<GetBookingDto?> GetBookingAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetAsync(bookingId);
            return _mapper.Map<GetBookingDto>(booking);
        }

        public async Task<GeneralReponse> RemoveBookingAsync(int bookingId)
        {
            return await _bookingRepository.RemoveAsync(bookingId);
        }
        //Memento
        public void SaveBookingState(BookingDraft booking)
        {
            if (booking == null) throw new ArgumentNullException(nameof(booking));

            _bookingHistory.SaveState(booking);
        }
        public BookingDraftMemento? GetCurrentBookingState()
        {
            if (!_bookingHistory.CanUndo()) return null;

            var currentState = _bookingHistory.GetUndoStack().Peek();
            return currentState;
        }
        public void ClearBookingHistory()
        {
            _httpContextAccessor.HttpContext!.Session.Clear();
            _httpContextAccessor.HttpContext!.Response.Cookies.Delete(".AspNetCore.Session", new CookieOptions
            {
                Path = "/",
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.None
            });
        }
        private GeneralReponse ValidateContactDetails(CreateContactDetailsDto contactDetails)
        {
            if (string.IsNullOrEmpty(contactDetails.Name) ||
                string.IsNullOrEmpty(contactDetails.Surname) ||
                string.IsNullOrEmpty(contactDetails.PhoneNumber) ||
                string.IsNullOrEmpty(contactDetails.Email)) return new GeneralReponse(false, "Misssing some contact data.");



            return new GeneralReponse(true,"Contact validated");
                
        }
        private GeneralReponse ValidatePassenger(CreatePassengerDto passenger)
        {
            if(string.IsNullOrEmpty(passenger.Name) || 
                string.IsNullOrEmpty(passenger.Surname) || 
                string.IsNullOrEmpty(passenger.Nationality)
               )
                return new GeneralReponse(false,"Missing some passenger data.");
            if (passenger.BirthDay.Date >= DateTime.Now.Date)
                return new GeneralReponse(false, "The birthday cannot be in present or future.");


            return new GeneralReponse(true,"Passenger created.");
        }
        private GeneralReponse ValidatePassport(CreatePassportDto passport) 
        {
            if (string.IsNullOrEmpty(passport.PassportNumber) ||
                string.IsNullOrEmpty(passport.Country)
              )
            {
                return new GeneralReponse(false, "Missing or invalid passport identity data.");
            }
            if (!passport.ExpiryDate.HasValue ||
                passport.ExpiryDate.Value.Date <= DateTime.Now.Date)
            {
                return new GeneralReponse(false, "The expiration passport date is not valid.");
            }

            return new GeneralReponse(true, "Passport data added");
        }

    }
}
