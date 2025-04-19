using System.ComponentModel.DataAnnotations.Schema;

namespace BaseEntity.Entities
{
    public class Itinerary
    {
        public int Id { get; set; }
        public int AirlineId { get; set; }
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public string? StopTime {  get; set; } = string.Empty;
        [NotMapped]
        public decimal TotalPrice => Segments?
            .Where(segment => segment.Flight != null)
            .Sum(segment => segment.Flight!.BasePrice) ?? 0m;

        [NotMapped]
        public DateTime DepartureDate => Segments?.FirstOrDefault()?.Flight?.DepartureDate ?? DateTime.MinValue;
        [NotMapped]
        public DateTime ArrivalDate => Segments?.LastOrDefault()?.Flight?.ArrivalDate ?? DateTime.MinValue;
        [NotMapped]
        public string DepartureTime => Segments?.FirstOrDefault()?.Flight?.DepartureTime ?? string.Empty;
        [NotMapped]
        public string ArrivalTime => Segments?.LastOrDefault()?.Flight?.ArrivalTime ?? string.Empty;
        [NotMapped]
        public string DurationTime => CalculateTotalDuration();
        [NotMapped]
        public bool HasStops => Segments?.Count() > 1;
        public ICollection<FlightSegment>? Segments { get; set; }
        public Airline? Airline { get; set; }

        private string CalculateTotalDuration()
        {
            if (Segments == null || !Segments.Any()) return "00:00";

            var firstDeparture = DepartureDate +
                            TimeSpan.Parse(DepartureTime);

            var lastArrival = ArrivalDate +
                            TimeSpan.Parse(ArrivalTime);

            var duration = lastArrival - firstDeparture;

            if(duration < TimeSpan.Zero) duration = duration.Add(TimeSpan.FromDays(1));

            return $"{(int)duration.TotalHours!}h {duration.Minutes}m";
        }
    }
}
