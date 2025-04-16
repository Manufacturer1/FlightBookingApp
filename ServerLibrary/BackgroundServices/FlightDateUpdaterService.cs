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
                    var today = now.Date;
                    var tomorrow = today.AddDays(1);
                    _logger.LogInformation($"Starting daily flight date update at {now}");

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var flightRepository = scope.ServiceProvider
                            .GetRequiredService<IFlightRepository>();

                        var flights = (await flightRepository.GetAllAsync()).ToList();
                        int updates = 0;
                        int tomorrowUpdates = 0;

                        foreach (var flight in flights)
                        {
                            bool isSpecialFlight = flight.FlightNumber == "16df11" || flight.FlightNumber == "292425";

                            if (isSpecialFlight)
                            {
                                
                                if (flight.DepartureDate.Date != tomorrow || flight.ArrivalDate.Date != tomorrow)
                                {
                                    flight.DepartureDate = tomorrow;
                                    flight.ArrivalDate = tomorrow;
                                    await flightRepository.UpdateAsync(flight);
                                    tomorrowUpdates++;
                                    updates++;
                                }
                            }
                            else
                            {
                               
                                if (flight.DepartureDate.Date != today || flight.ArrivalDate.Date != today)
                                {
                                    flight.DepartureDate = today;
                                    flight.ArrivalDate = today;
                                    await flightRepository.UpdateAsync(flight);
                                    updates++;
                                }
                            }
                        }

                        if (updates > 0)
                        {
                            _logger.LogInformation($"Updated {updates} flights ({tomorrowUpdates} to tomorrow) at {now}");
                        }
                        else
                        {
                            _logger.LogInformation("No update required");
                        }
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
