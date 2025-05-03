using ServerLibrary.Command;

namespace ServerLibrary.FactoryMethod
{
    public interface ICreateCommand<T> where T : IBookingCommand
    {
        IBookingCommand CreateCommand(params object[] args); 
    }
}
