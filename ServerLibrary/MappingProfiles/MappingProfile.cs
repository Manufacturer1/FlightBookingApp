using AutoMapper;
using BaseEntity.Dtos;
using BaseEntity.Entities;

namespace ServerLibrary.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //From dto to entity

            CreateMap<CreateFlightDto, Flight>()
                .ForMember(dest => dest.DestinationImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Plane, opt => opt.Ignore())
                .ForMember(dest => dest.FlightNumber, opt => opt.Ignore());
                

            CreateMap<UpdateAirlineDto, Airline>()
                .ForAllMembers(opt => opt.Condition((src, srcMember) => srcMember != null));





            CreateMap<CreateAirlineDto, Airline>()
                .ForMember(dest => dest.AirlineImageUrl,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore())
                .ForMember(dest => dest.BaggagePolicy, opt => opt.Ignore())
                .ForMember(dest => dest.Itineraries, opt => opt.Ignore());

            CreateMap<UpdateAirlineDto, Airline>()
                .ForMember(dest => dest.AirlineImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.BaggagePolicy, opt => opt.Ignore())
                .ForMember(dest => dest.Itineraries, opt => opt.Ignore());



            CreateMap<CreatePlaneDto, Plane>()
                .ForMember(dest => dest.Flights, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
  

            CreateMap<CreateBaggageDto, BaggagePolicy>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Airlines, opt => opt.Ignore());



            CreateMap<CreateItineraryDto, Itinerary>()
                .ForMember(dest => dest.Segments, opt => opt.Ignore())
                .ForMember(dest => dest.Airline, opt => opt.Ignore());
    

            CreateMap<FlightSegmentRequest, FlightSegment>()
                .ForMember(dest => dest.SegmentOrder, opt => opt.Ignore())
                .ForMember(dest => dest.Flight, opt => opt.Ignore())
                .ForMember(dest => dest.Itinerary, opt => opt.Ignore())
                .ForMember(dest => dest.ItineraryId, opt => opt.Ignore());

            CreateMap<CreateAirportDto, Airport>()
                .ForMember(dest => dest.DepartingFlights, opt => opt.Ignore())
                .ForMember(dest => dest.ArrivalFlights, opt => opt.Ignore());

            CreateMap<CreateAmenityDto, Amenity>()
                .ForMember(dest => dest.FlightAmenities, opt => opt.Ignore());


            CreateMap<FlightAmenityDto, FlightAmenity>()
                .ForMember(dest => dest.Flight, opt => opt.Ignore())
                .ForMember(dest => dest.Amenity, opt => opt.Ignore())
                .ForMember(dest => dest.AmenityId, opt => opt.Ignore());

            CreateMap<UpdateAmenityDto, Amenity>();

            CreateMap<CreateDiscountDto, Discount>();
            CreateMap<UpdateDiscountDto, Discount>();
                
            CreateMap<CreateContactDetailsDto, ContactDetails>();
            CreateMap<CreatePassportDto, PassportIdentity>();
            CreateMap<CreatePassengerDto, Passenger>();
            CreateMap<CreateBookingDto, Booking>();

            CreateMap<MarkNotificationReadDto, Notification>();

            //From entity to dto
            CreateMap<Flight, GetFlightDto>();
            
            CreateMap<Flight,FlightCardResponseDto>();

            CreateMap<Flight, UpdateFlightDto>()
                .ForMember(dest => dest.DestinationImage, opt => opt.Ignore());


            CreateMap<Airline, GetAirlineDto>();

            CreateMap<Airline, UpdateAirlineDto>()
                .ForMember(dest => dest.AirlineImage, opt => opt.Ignore());

            CreateMap<Plane,GetPlaneDto>();
            CreateMap<BaggagePolicy,GetBaggageDto>();

            CreateMap<Itinerary, GetItineraryDto>();
            CreateMap<FlightSegment, GetFlightSegmentDto>();

            CreateMap<Airport, GetAirportDto>();

            CreateMap<Amenity, GetAmenityDto>();

            CreateMap<FlightAmenity, FlightAmenityDto>();

            CreateMap<Discount, GetDiscountDto>();

            CreateMap<ContactDetails, GetContactDetailsDto>();
            CreateMap<PassportIdentity,GetPassportDto>();
            CreateMap<Passenger, CreatePassengerDto>();
            CreateMap<Booking, GetBookingDto>();
            CreateMap<Notification, GetNotificationDto>();
        }
    }
}
