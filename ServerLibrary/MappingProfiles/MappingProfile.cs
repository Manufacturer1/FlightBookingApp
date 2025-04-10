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
                .ForMember(dest => dest.Airline, opt => opt.Ignore())
                .ForMember(dest => dest.Plane, opt => opt.Ignore())
                .ForMember(dest => dest.FlightNumber, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Airline = null;
                    dest.Plane = null;
                });

            CreateMap<UpdateFlightDto, Flight>()
                .ForMember(dest => dest.DestinationImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Airline, opt => opt.Ignore())
                .ForMember(dest => dest.Plane, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Airline = null;
                    dest.Plane = null;
                });


            CreateMap<CreateAirlineDto, Airline>()
                .ForMember(dest => dest.AirlineImageUrl,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Flights,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore())
                .ForMember(dest => dest.BaggagePolicy,opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Flights = null;
                    dest.BaggagePolicy = null;
                });

            CreateMap<UpdateAirlineDto, Airline>()
                .ForMember(dest => dest.AirlineImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Flights, opt => opt.Ignore())
                .ForMember(dest => dest.BaggagePolicy,opt => opt.Ignore())  
                .AfterMap((src, dest) =>
                {
                    dest.BaggagePolicy = null;
                    dest.Flights = null;
                });


            CreateMap<CreatePlaneDto, Plane>()
                .ForMember(dest => dest.Flights, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Flights = null;
                });

            CreateMap<CreateBaggageDto, BaggagePolicy>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Airline, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Airline = null;
                });


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
            

            
        }
    }
}
