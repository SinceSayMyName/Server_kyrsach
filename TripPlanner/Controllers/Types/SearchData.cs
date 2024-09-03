namespace TripPlanner.Controllers.Types
{
    public class SearchData
    {
        public int TransportTypeID { get; set; }
        public string DepartureCityCode { get; set; } = "";
        public string ArrivalCityCode { get; set; } = "";
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public int SeatsNumber { get; set; }
    }
}