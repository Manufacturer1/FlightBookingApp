using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServerLibrary.Repositories.Interfaces;

namespace ServerLibrary.BackgroundServices
{
    public class ItineraryBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ItineraryBackgroundService> _logger;
        private readonly TimeSpan _updateInterval = TimeSpan.FromDays(1);
        private readonly TimeSpan _initialDelay = TimeSpan.FromSeconds(10);

        private readonly Dictionary<int, int> updatedFlightOffsets = new Dictionary<int, int>
        {
            {11, 0},
            {12, 1},
            {13, 2},
            {14, 3},
            {1014, 4},
            {1015, 0},
            {1016, 5},
            {1017,0 },
            {1018,0 }
        };

        public ItineraryBackgroundService(IServiceProvider serviceProvider,
                                   ILogger<ItineraryBackgroundService> logger)
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
                    using var scope = _serviceProvider.CreateScope();
                    var flightRepository = scope.ServiceProvider.GetRequiredService<IFlightRepository>();
                    var itineraryRepository = scope.ServiceProvider.GetRequiredService<IItineraryRepository>();
                    int updates = 0;

                    var today = DateTime.Today;

                foreach (var kvp in updatedFlightOffsets)
                {
                    int itineraryId = kvp.Key;
                    DateTime outboundDate = today.AddDays(kvp.Value);
                    DateTime returnDate = outboundDate.AddDays(1); 

                    var itinerary = await itineraryRepository.GetByIdAsync(itineraryId);
                    if (itinerary?.Segments == null) continue;

                    foreach (var segment in itinerary.Segments)
                    {
                        var flight = segment.Flight;
                        if (flight == null) continue;

                        var targetDate = segment.IsReturnSegment ? returnDate : outboundDate;

                        if (flight.DepartureDate.Date != targetDate || flight.ArrivalDate.Date != targetDate)
                        {
                            flight.DepartureDate = targetDate;
                            flight.ArrivalDate = targetDate;
                            await flightRepository.UpdateAsync(flight);
                            updates++;
                        }
                    }
                }



                    if (updates > 0)
                    {
                        _logger.LogInformation($"Updated {updates} flights.");
                    }
                    else
                    {
                        _logger.LogInformation("No updates were required.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred during daily flight date update.");
                }

                await Task.Delay(_updateInterval, stoppingToken);
            }

            _logger.LogInformation("Daily Flight Date Updater Service is stopping.");
        }
    }
}
