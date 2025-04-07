using BaseEntity.CustomValidations;
using ServerLibrary.Enums;
using System.ComponentModel.DataAnnotations;

namespace BaseEntity.Dtos
{
    public class FlightCardRequestDto
    {
        [Required]
        public string Origin { get; set; } = string.Empty;

        [Required]
        public string Destination {  get; set; } = string.Empty;
        public ClassType ClassType { get; set; } = ClassType.Economy;
        public int PassengerNumber { get; set; }
        public TripType TripType {  get; set; } = TripType.OneWay;
        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        public DateTime DepartureDate {  get; set; } 

        [ArrivalAfterDeparture]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        public DateTime? ReturnDate { get; set; }
    }
}
