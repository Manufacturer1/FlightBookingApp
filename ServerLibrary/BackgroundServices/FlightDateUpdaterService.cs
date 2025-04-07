using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServerLibrary.Repositories.Interfaces;

namespace ServerLibrary.BackgroundServices
{
    public class FlightDateUpdaterService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<FlightDateUpdaterService> _logger;
        private readonly TimeSpan _updateInterval = TimeSpan.FromDays(1);
        private readonly TimeSpan _initialDelay = TimeSpan.FromSeconds(10);

        public FlightDateUpdaterService(IServiceProvider serviceProvider,
                                      ILogger<FlightDateUpdaterService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Daily Flight Date Updater Service is starting.");


            await Task.Delay(_initialDelay, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.Now;
                    _logger.LogInformation($"Starting daily flight date update at {now}");

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var flightRepository = scope.ServiceProvider
                            .GetRequiredService<IFlightRepository>();

                    
                        var flights = await flightRepository.GetAllAsync();
                        int updates = 0;

                        foreach (var flight in flights)
                        {
                            if(flight.DepartureDate.Date < now.Date)
                            {
                                flight.DepartureDate = now;
                                flight.ArrivalDate = now.AddDays(1);

                                await flightRepository.UpdateAsync(flight);
                                updates++;

                            }   
                        }

                        if (updates > 0)
                        {
                            _logger.LogInformation($"Updated {flights.Count()} flights at {now}");

                        }
                        else
                            _logger.LogInformation($"No update required");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred during daily flight date update");
                }


                await Task.Delay(_updateInterval, stoppingToken);
            }

            _logger.LogInformation("Daily Flight Date Updater Service is stopping.");
        }
    }
}
