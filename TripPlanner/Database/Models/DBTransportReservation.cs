using System.ComponentModel.DataAnnotations.Schema;

namespace TripPlanner.DBTripPlanner.Models
{
    public class DBTransportReservation : DBEntity
    {
        public int? UserId { get; set; }
        public string? UserEmail { get; set; }
        public int PassengerCount { get; set; }
        public bool Paid { get; set; }
        public bool LuggageRequired { get; set; }
        public virtual DBUser? User { get; set; }
    }
}
