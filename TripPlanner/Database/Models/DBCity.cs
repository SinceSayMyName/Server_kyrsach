using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TripPlanner.DBTripPlanner.Models
{
    public class DBCity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string FiasCode { get; set; }
        public string Name { get; set; }
    }
}
