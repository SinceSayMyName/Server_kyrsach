using System.ComponentModel.DataAnnotations.Schema;

namespace TripPlanner.DBTripPlanner.Models
{
    public class DBTransportOption: DBEntity
    {
        public int? CompanyId { get; set; }
        public int? TransportId { get; set; }
        public string DepartureCityFiasCode { get; set; }
        public string ArrivalCityFiasCode { get; set; }

        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public int SeatsLeft { get; set; }
        public double Price { get; set; }
        public double PriceWithLuggage { get; set; }

        public virtual DBCity DepartureCity { get; set; }
        public virtual DBCity ArrivalCity { get; set; }
        public virtual DBCompany Company { get; set; }
        public virtual DBTransport Transport { get; set; }
    }
}
