using BaseEntity.Entities;
using BaseEntity.Responses;
using ServerLibrary.Command.CommandResults;
using ServerLibrary.Repositories.Interfaces;

namespace ServerLibrary.Command
{
    public class ValidateItineraryCommand : IBookingCommand
    {
        private readonly IItineraryRepository _itineraryRepository;
        private readonly int _itineraryId;
        
        public Itinerary? ValidatedItinerary { get; private set; }

        public ValidateItineraryCommand(IItineraryRepository itineraryRepository,int itineraryId)
        {
            _itineraryRepository = itineraryRepository;
            _itineraryId = itineraryId;
        }
       
        public async Task<CommandResult> ExecuteAsync()
        {
            ValidatedItinerary = await _itineraryRepository.GetByIdAsync(_itineraryId);
            return ValidatedItinerary == null
                ? CommandResult.Failure("Itinerary does not exist.")
                : CommandResult.Success();
        }

    }
}
