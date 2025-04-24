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

        public BookingService(IBookingRepository bookingRepository,
            IFlightRepository flightRepository,
            IItineraryRepository itineraryRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IPassengerRepository passengerRepository,
            IPassportIdentityRepository passportIdentityRepository,
            IContactRepository contactRepository)
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
        }


        public async Task<GeneralReponse> BookSeatAsync(CreateBookingDto createBooking)
        {
            // Step 1: Validate itinerary
            var itinerary = await _itineraryRepository.GetByIdAsync(createBooking.ItineraryId);
            if (itinerary == null)
                return new GeneralReponse(false, "Itinerary does not exist.");

            // Step 2: Load current booking draft from Memento
            var draft = GetCurrentBookingState();
            if (draft == null)
                return new GeneralReponse(false, "Booking history missing");

            // Step 3: Validate input sections
            if (draft.ContactDetails == null || draft.Passenger == null || draft.Passport == null)
                return new GeneralReponse(false, "Incomplete booking draft");

            var contactValidation = ValidateContactDetails(draft.ContactDetails);
            if (!contactValidation.Flag) return contactValidation;

            var passengerValidation = ValidatePassenger(draft.Passenger);
            if (!passengerValidation.Flag) return passengerValidation;

            var passportValidation = ValidatePassport(draft.Passport);
            if (!passportValidation.Flag) return passportValidation;

            if (string.IsNullOrEmpty(createBooking.PaymentIntentId))
                return new GeneralReponse(false, "Payment intent is null.");

            // Step 4: Map and persist entities
            try
            {
                var contact = _mapper.Map<ContactDetails>(draft.ContactDetails);
                var contactId = await _contactRepository.CreateAsync(contact);
       

                var passport = _mapper.Map<PassportIdentity>(draft.Passport);
                var passportId = await _passportIdentityRepository.CreateAsync(passport);
       

                var passenger = _mapper.Map<Passenger>(draft.Passenger);
                passenger.ContactDetailsId = contactId;
                passenger.PassportIdentityId = passportId;
                var passengerId = await _passengerRepository.CreateAsync(passenger);
           

                // Step 5: Create booking
                var booking = _mapper.Map<Booking>(createBooking);
                booking.PassengerId = passengerId;
                booking.BookingDate = DateTime.Now;

                // Step 6: Update seat availability
                foreach (var segment in itinerary.Segments!)
                {
                    var existingFlight = await _flightRepository.GetByFlightNumberAsync(segment.FlightNumber);
                    if (existingFlight == null)
                        return new GeneralReponse(false, $"Flight {segment.FlightNumber} does not exist.");

                    if (existingFlight.AvailableSeats < createBooking.PassengerNumberSelected)
                        return new GeneralReponse(false, $"Not enough seats on flight {segment.FlightNumber}.");

                    existingFlight.AvailableSeats -= createBooking.PassengerNumberSelected;
                    await _flightRepository.UpdateAsync(existingFlight);
                }

                // Step 7: Save booking
                return await _bookingRepository.CreateAsync(booking);
            }
            catch(Exception ex)
            {
                return new GeneralReponse(false, ex.Message);
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
            _httpContextAccessor.HttpContext!.Response.Cookies.Delete(".AspNetCore.Session");
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
