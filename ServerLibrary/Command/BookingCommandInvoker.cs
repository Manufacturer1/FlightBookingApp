using BaseEntity.Responses;
using System.Transactions;

namespace ServerLibrary.Command
{
    public class BookingCommandInvoker
    {
        private readonly List<IBookingCommand> _commands = new List<IBookingCommand>();
        public void AddCommand(IBookingCommand command) => _commands.Add(command);

        public async Task<BookingResponse> ExecuteAllAsync()
        {
            using(var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                try
                {
                    foreach(var command in _commands)
                    {
                        var result = await command.ExecuteAsync();
                        if (!result.Flag)
                            return new BookingResponse(false, result.Message!, null);
                    }
                    scope.Complete();
                    return new BookingResponse(true, "Booking Completed successfully", null);
                }
                catch (Exception ex)
                {
                    return new BookingResponse(false,ex.Message!, null);
                }
            }
        }
    }
}
