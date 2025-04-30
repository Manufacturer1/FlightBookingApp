using BaseEntity.Entities;
using ServerLibrary.Command.CommandResults;
using ServerLibrary.Repositories.Interfaces;

namespace ServerLibrary.Command
{
    public class UpdateSeatAvailabilityCommand : IBookingCommand
    {
        private readonly IFlightRepository _flightRepo;
        private readonly ValidateItineraryCommand _itineraryCommand;
        private readonly int _passengerSelected;
        public UpdateSeatAvailabilityCommand(
              IFlightRepository flightRepo,ValidateItineraryCommand itineraryCommand, int passengerSelected)
        {
            _flightRepo = flightRepo;
            _itineraryCommand = itineraryCommand;
            _passengerSelected = passengerSelected;
        }
        public async Task<CommandResult> ExecuteAsync()
        {
            var segments = _itineraryCommand.ValidatedItinerary?.Segments;
            if (segments == null)
                return CommandResult.Failure("Itinerary does not exist.");
            foreach (var segment in segments)
            {
                var flight = await _flightRepo.GetByFlightNumberAsync(segment.FlightNumber);
                if (flight == null)
                    return CommandResult.Failure($"Flight {segment.FlightNumber} does not exist.");

                if (flight.AvailableSeats < _passengerSelected)
                    return CommandResult.Failure($"Not enough seats on flight {segment.FlightNumber}.");

                flight.AvailableSeats -= _passengerSelected;
                await _flightRepo.UpdateAsync(flight);
            }

            return CommandResult.Success();
        }
    }
}
