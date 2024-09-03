using Microsoft.EntityFrameworkCore;
using TripPlanner.Controllers.Types;
using TripPlanner.Database.Services;
using TripPlanner.DBTripPlanner;
using TripPlanner.DBTripPlanner.Models;
using TripPlanner.DBTripPlanner.Services;

namespace TripPlanner.Controllers
{
    public class Converter
    {
        private DBCompanyService _dBCompanyService;
        private DBTransportService _dBTransportService;
        private DBTransportOptionService _dBTransportOptionService;
        private DBTransportOptionTransportReservationRelationService _dBTransportOptionTransportReservationRelationService;

        public Converter(DBApplicationContext dBApplicationContext)
        {
            _dBCompanyService = new DBCompanyService(dBApplicationContext);
            _dBTransportService = new DBTransportService(dBApplicationContext);
            _dBTransportOptionService = new DBTransportOptionService(dBApplicationContext);
            _dBTransportOptionTransportReservationRelationService = new DBTransportOptionTransportReservationRelationService(dBApplicationContext);
        }

        public User GetUserFromDBUser(DBUser dBUser)
        {
            return new User
            {
                Id = dBUser.Id,
                Phone = dBUser.Phone,
                Email = dBUser.Email,
                Password = dBUser.Password,
                IsAdmin = dBUser.IsAdmin,
                DelayNotification = dBUser.DelayNotification
            };
        }
        public DBUser GetDBUserFromUser(User user)
        {
            DBUser updatedUser =  new DBUser
            {
                Phone = user.Phone ?? "",
                Email = user.Email,
                Login = user.Email,
                Password = user.Password,
                IsAdmin = user.IsAdmin,
                DelayNotification = user.DelayNotification
            };
            if (user.Id != null)
            {
                updatedUser.Id = user.Id ?? -1;
            }
            return updatedUser;
        }

        public TransportOption GetTransportOptionFromDBTransportOption(DBTransportOption transportOption)
        {
            return new TransportOption
            {
                Id = transportOption.Id,
                Transport = _dBTransportService.GetById(transportOption.TransportId),
                DepartureCityCode = transportOption.DepartureCityFiasCode,
                ArrivalCityCode = transportOption.ArrivalCityFiasCode,
                Company = _dBCompanyService.GetById(transportOption.CompanyId),
                DepartureDate = transportOption.DepartureDate,
                ArrivalDate = transportOption.ArrivalDate,
                Price = transportOption.Price,
                PriceWithLuggage = transportOption.PriceWithLuggage
            };
        }

        public DBTransportOption GetDBTransportOptionFromTransportOption(TransportOption transportOption)
        {
            return new DBTransportOption
            {
                Id = transportOption.Id,
                TransportId = transportOption.Transport.Id,
                DepartureCityFiasCode = transportOption.DepartureCityCode,
                ArrivalCityFiasCode = transportOption.ArrivalCityCode,
                CompanyId = transportOption.Company.Id,
                DepartureDate = transportOption.DepartureDate,
                ArrivalDate = transportOption.ArrivalDate,
                Price = transportOption.Price,
                PriceWithLuggage = transportOption.PriceWithLuggage
            };
        }

        public TransportReservation GetTransportReservationFromDBTransportReservation(DBTransportReservation transportReservation)
        {
            int[] optionsID = _dBTransportOptionTransportReservationRelationService.GetByTransportReservationID(transportReservation.Id).Select(x => x.TransportOptionId).ToArray();
            TransportOption[] options = optionsID.Select(x => GetTransportOptionFromDBTransportOption(_dBTransportOptionService.GetById(x))).ToArray();

            return new TransportReservation
            {
                Id = transportReservation.Id,
                PassengerCount = transportReservation.PassengerCount,
                Paid = transportReservation.Paid,
                TransportOptions = options,
                WithLuggage = transportReservation.LuggageRequired
            };
        }
    }
}
