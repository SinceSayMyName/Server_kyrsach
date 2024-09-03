using TripPlanner.DBTripPlanner.Models;

namespace TripPlanner.Controllers.Types
{
    public class TransportOption
    {
        public int? Id { get; set; }
        public DBTransport Transport { get; set; }
        public string DepartureCityCode { get; set; }
        public string ArrivalCityCode { get; set; }
        public DBCompany Company { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public double Price { get; set; }
        public double PriceWithLuggage { get; set; }
    }
}