using BaseEntity.Enums;
using ServerLibrary.Command;

namespace ServerLibrary.FactoryMethod
{
    public interface IBookingCommandCreatorFactory
    {
        ICreateCommand<T> CreateBookingCommand<T>(BookingCommandType commandType) where T : IBookingCommand;
    }
}
