namespace TripPlanner.DBTripPlanner.Models
{
    public class DBUser : DBEntity
    {
        public string Login { get; set; } = "";
        public string Password { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public bool IsAdmin { get; set; } = false;
        public bool DelayNotification { get; set; } = false;
    }
}
