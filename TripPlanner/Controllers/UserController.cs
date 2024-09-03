using Microsoft.AspNetCore.Mvc;
using TripPlanner.Controllers.Types;
using TripPlanner.DBTripPlanner;
using TripPlanner.DBTripPlanner.Models;
using TripPlanner.DBTripPlanner.Services;

namespace TripPlanner.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly Converter _converter;
        private DBUserService _dBUserService;
        private DBTransportReservationService _dBTransportReservationService;

        public UserController(ILogger<UserController> logger, Converter converter, DBApplicationContext dBApplicationContext)
        {
            _logger = logger;
            _converter = converter;
            _dBUserService = new DBUserService(dBApplicationContext);
            _dBTransportReservationService = new DBTransportReservationService(dBApplicationContext);
        }

        [HttpGet("GetUser")]
        public User GetUser(string login, string password)
        {
            DBUser? dBUser = _dBUserService.Authenticate(login, password);
            if (dBUser == null)
            {
                return null;
            }
            return _converter.GetUserFromDBUser(dBUser);
        }

        [HttpPost("UpdateUser")]
        public User UpdateUser([FromBody] User user)
        {
            DBUser? dBUser = _dBUserService.GetById(user.Id);
            DBUser updatedDBUser = _converter.GetDBUserFromUser(user);
            if (dBUser == null)
            {
                updatedDBUser = _dBUserService.Create(updatedDBUser);
            } else
            {
                updatedDBUser = _dBUserService.Update(updatedDBUser);
            }
            return _converter.GetUserFromDBUser(updatedDBUser);
        }

        [HttpGet("GetUserReservations")]
        public List<TransportReservation> GetUserReservations(int userID)
        {
            return _dBTransportReservationService.GetByUserID(userID)?.Select(x => _converter.GetTransportReservationFromDBTransportReservation(x)).ToList() ?? new List<TransportReservation>();
        }
    }
}