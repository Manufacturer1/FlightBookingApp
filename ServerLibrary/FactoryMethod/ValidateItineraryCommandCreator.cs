using ServerLibrary.Command;
using ServerLibrary.Repositories.Interfaces;

namespace ServerLibrary.FactoryMethod
{
    public class ValidateItineraryCommandCreator : ICreateCommand<ValidateItineraryCommand>
    {
        private readonly IItineraryRepository _itineraryRepo;

        public ValidateItineraryCommandCreator(IItineraryRepository itineraryRepository)
        {
            _itineraryRepo = itineraryRepository;
        }

        public IBookingCommand CreateCommand(params object[] args)
        {
            int itineraryId = (int)args[0];
            return new ValidateItineraryCommand(_itineraryRepo, itineraryId);
        }
    }
}
