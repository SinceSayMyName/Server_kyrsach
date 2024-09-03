using System.ComponentModel.DataAnnotations.Schema;
using TripPlanner.DBTripPlanner.Models;

namespace TripPlanner.Database.Models
{
    public class DBTransportOptionTransportReservationRelation : DBEntity
    {
        public int TransportOptionId { get; set; }
        public int TransportReservationId { get; set; }

        public virtual DBTransportOption TransportOption { get; set; }
        public virtual DBTransportReservation TransportReservation { get; set; }
    }
}
