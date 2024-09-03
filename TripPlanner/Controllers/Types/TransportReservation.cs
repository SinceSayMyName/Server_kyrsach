namespace TripPlanner.Controllers.Types
{
    public class TransportReservation
    {
        public int? Id { get; set; }
        public TransportOption[] TransportOptions { get; set; }
        public int PassengerCount { get; set; }
        public bool Paid { get; set; }
        public bool WithLuggage { get; set; }
    }
}