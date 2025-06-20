﻿using BaseEntity.Dtos;
using BaseEntity.Responses;

namespace ServerLibrary.Services.Interfaces
{
    public interface IFlightService
    {
        Task<GeneralReponse> AddAsync(CreateFlightDto createFlight);
        Task<GeneralReponse> UpdateAsync(UpdateFlightDto updateFlight);
        Task<GeneralReponse> DeleteAsync(string flightNumber);
        Task<GetFlightDto?> GetByFlightNumberAsync(string flightNumber);
        Task<IEnumerable<GetFlightDto>> GetAllAsync();
        Task<IEnumerable<ItineraryCardResponseDto>> GetFlightCards(FlightCardRequestDto flightCardRequest,bool withoutDate = false);

    }
}
