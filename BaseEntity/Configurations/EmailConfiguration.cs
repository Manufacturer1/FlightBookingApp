namespace BaseEntity.Configurations
{
    public class EmailConfiguration
    {
        private static EmailConfiguration? _instance;
        private static readonly object _lock = new object();

        private EmailConfiguration() { }

        public static EmailConfiguration Instance
        {
            get
            {
                if (_instance == null)
                    throw new InvalidOperationException("Email configuration not instantiated.Call initialise first.");
                return _instance;
            }
        }
        public static void Initialize(string host,int port,string email,string password)
        {
            lock (_lock)
            {
                if (_instance != null)
                    throw new InvalidOperationException("EmailConfiguration already initialized.");

                _instance = new EmailConfiguration
                {
                    Host = host,
                    Port = port,
                    Email = email,
                    Password = password
                };
            }
        }
        public string? Host { get; private set; }
        public int Port { get; private set; }
        public string? Email { get; private set; }
        public string? Password { get; private set; }
    }
}
