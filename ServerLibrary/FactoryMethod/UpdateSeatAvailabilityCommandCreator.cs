using ServerLibrary.Command;
using ServerLibrary.Repositories.Interfaces;

namespace ServerLibrary.FactoryMethod
{
    public class UpdateSeatAvailabilityCommandCreator : ICreateCommand<UpdateSeatAvailabilityCommand>
    {

        private readonly IFlightRepository _flightRepo;

        public UpdateSeatAvailabilityCommandCreator(IFlightRepository flightRepo)
        {
            _flightRepo = flightRepo;
        }
        public IBookingCommand CreateCommand(params object[] args)
        {
            var itineraryCmd = (ValidateItineraryCommand)args[0];
            int passengerNumber = (int)args[1];
            return new UpdateSeatAvailabilityCommand(_flightRepo, itineraryCmd, passengerNumber);
        }
    }
}
