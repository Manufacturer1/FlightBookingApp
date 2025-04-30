namespace ServerLibrary.Command.CommandResults
{
    public class CommandResult
    {
        public bool Flag { get; }
        public string? Message { get; }
        public object? Data { get; }

        private CommandResult(bool success, string message, object data)
        {
            Flag = success;
            Message = message;
            Data = data;
        }

        public static CommandResult Success(object data = null!) => new CommandResult(true, null!, data);
        public static CommandResult Failure(string message) => new CommandResult(false, message, null!);
    }
}
