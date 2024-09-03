using TripPlanner.DBTripPlanner.Models;
using TripPlanner.DBTripPlanner.Services;

namespace TripPlanner.DBTripPlanner
{
    public class DBHelper
    {
        private DBTransportOptionService _dBTransportOptionService;
        private DBTransportReservationService _dBTransportReservationService;
        private DBCompanyService _dBCompanyService;
        private DBTransportTypeService _dBTransportTypeService;
        private DBUserService _dBUserService;
        private DBCityService _dBCityService;
        private DBTransportService _dBTransportService;

        public DBHelper(DBApplicationContext dBApplicationContext)
        {
            _dBCompanyService = new DBCompanyService(dBApplicationContext);
            _dBTransportOptionService = new DBTransportOptionService(dBApplicationContext);
            _dBTransportReservationService = new DBTransportReservationService(dBApplicationContext);
            _dBTransportTypeService = new DBTransportTypeService(dBApplicationContext);
            _dBTransportService = new DBTransportService(dBApplicationContext);
            _dBCityService = new DBCityService(dBApplicationContext);
            _dBUserService = new DBUserService(dBApplicationContext);
        }

        public void GenerateTestDataIfEmpty()
        {
            if (_dBTransportTypeService.GetAll()?.Count == 0)
            {
                _dBTransportTypeService.Create(new DBTransportType { Name = "Самолет" });
                _dBTransportTypeService.Create(new DBTransportType { Name = "Поезд" });
            }
            if (!Constants.GenerateTestData) { return; }
            if (_dBCompanyService.GetAll()?.Count == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    _dBCompanyService.Create(CreateTestDBCompany(i));
                }
            }
            if (_dBUserService.GetAll()?.Count == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    _dBUserService.Create(CreateTestDBUser(i));
                }
            }
            if (_dBTransportService.GetAll()?.Count == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    _dBTransportService.Create(CreateTestDBTransport(i));
                }
            }
            if (_dBTransportOptionService.GetAll()?.Count == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    _dBTransportOptionService.Create(CreateTestDBTransportOption(i));
                }
            }
            if (_dBTransportReservationService.GetAll()?.Count == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    _dBTransportReservationService.Create(CreateTestDBTransportResevation(i));
                }
            }
        }

        public DBCompany CreateTestDBCompany(int number)
        {
            return new DBCompany
            {
                Name = "Name" + number
            };
        }

        public DBUser CreateTestDBUser(int number)
        {
            return new DBUser
            {
                Login = "Login" + number,
                Password = "Password" + number,
                Phone = "Phone" + number,
                Email = "Email" + number,
                DelayNotification = number % 2 == 0
            };
        }

        public DBTransport CreateTestDBTransport(int number)
        {
            return new DBTransport
            {
                SeatsCount = number,
                Name = "Name" + number,
                Type = _dBTransportTypeService.GetAll()?.ToArray()[0],
            };
        }

        public DBTransportOption CreateTestDBTransportOption(int number)
        {
            return new DBTransportOption
            {
                Transport = _dBTransportService.GetAll()?.ToArray()[0],
                DepartureCity = (number % 2 == 0 ? _dBCityService.GetAll()?.ToArray()[0] : _dBCityService.GetAll()?.ToArray()[1]),
                ArrivalCity = (number % 2 != 0 ? _dBCityService.GetAll()?.ToArray()[0] : _dBCityService.GetAll()?.ToArray()[1]),
                Company = _dBCompanyService.GetAll()?.ToArray()[0],
                DepartureDate = DateTime.Now,
                ArrivalDate = DateTime.Now,
                SeatsLeft = number * 2,
                Price = number
            };
        }

        public DBTransportReservation CreateTestDBTransportResevation(int number)
        {
            return new DBTransportReservation
            {
                User = _dBUserService.GetAll()?.ToArray()[0],
                PassengerCount = number,
                Paid=false
            };
        }
    }
}