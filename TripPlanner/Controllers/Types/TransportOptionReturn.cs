namespace TripPlanner.Controllers.Types
{
    public class TransportOptionReturn
    {
        public List<List<TransportOption>> transportOptionTo { get; set; }
        public List<List<TransportOption>> transportOptionFrom { get; set; }
    }
}
