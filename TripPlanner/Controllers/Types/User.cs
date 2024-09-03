namespace TripPlanner.Controllers.Types
{
    public class User
    {
        public int? Id { get; set; }
        public string? Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool DelayNotification { get; set; }
        public bool IsAdmin { get; set; }
    }
}