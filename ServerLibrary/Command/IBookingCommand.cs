using ServerLibrary.Command.CommandResults;

namespace ServerLibrary.Command
{
    public interface IBookingCommand
    {
        Task<CommandResult> ExecuteAsync();
    }
}
