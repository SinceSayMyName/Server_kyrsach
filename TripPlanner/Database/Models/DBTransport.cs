using System.ComponentModel.DataAnnotations.Schema;

namespace TripPlanner.DBTripPlanner.Models
{
    public class DBTransport : DBEntity
    {
        public int? TypeId { get; set; }
        public string? Name { get; set; }
        public int? SeatsCount { get; set; }
        public virtual DBTransportType? Type { get; set; }
    }
}