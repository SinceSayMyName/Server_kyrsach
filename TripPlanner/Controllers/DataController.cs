using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TripPlanner.Controllers.Types;
using TripPlanner.Database.Models;
using TripPlanner.Database.Services;
using TripPlanner.DBTripPlanner;
using TripPlanner.DBTripPlanner.Models;
using TripPlanner.DBTripPlanner.Services;
using TripPlanner.EMailNotifications;

namespace TripPlanner.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {

        private readonly ILogger<DataController> _logger;
        private readonly Converter _converter;
        private DBTransportTypeService _dBTransportTypeService;
        private DBCityService _dBCityService;
        private DBTransportOptionService _dBTransportOptionService;
        private DBCompanyService _dBCompanyService;
        private DBUserService _dBUserService;
        private DBTransportService _dBTransportService;
        private DBTransportReservationService _dBReservationService;
        private DBTransportOptionTransportReservationRelationService _dBTransportOptionTransportReservationRelationService;
        private EmailMessageSender _emailMessageSender;

        public DataController(ILogger<DataController> logger, Converter converter, DBApplicationContext dBApplicationContext)
        {
            _logger = logger;
            _converter = converter;
            _dBTransportTypeService = new DBTransportTypeService(dBApplicationContext);
            _dBCityService = new DBCityService(dBApplicationContext);
            _dBUserService = new DBUserService(dBApplicationContext);
            _dBCompanyService = new DBCompanyService(dBApplicationContext);
            _dBTransportService = new DBTransportService(dBApplicationContext);
            _dBTransportOptionService = new DBTransportOptionService(dBApplicationContext);
            _dBReservationService = new DBTransportReservationService(dBApplicationContext);
            _dBTransportOptionTransportReservationRelationService = new DBTransportOptionTransportReservationRelationService(dBApplicationContext);

            _emailMessageSender = new EmailMessageSender(dBApplicationContext, converter);
        }

        [HttpGet("GetTransportTypes")]
        public List<DBTransportType> GetTransportTypes()
        {
            return _dBTransportTypeService.GetAll() ?? new List<DBTransportType>();
        }

        [HttpGet("GetCities")]
        public List<DBCity> GetCities()
        {
            return _dBCityService.GetAll()?.ToList() ?? new List<DBCity>();
        }

        [HttpGet("GetCompanies")]
        public List<DBCompany> GetCompanies()
        {
            return _dBCompanyService.GetAll()?.ToList() ?? new List<DBCompany>();
        }

        [HttpGet("GetTransports")]
        public List<DBTransport> GetTransports()
        {
            return _dBTransportService.GetAll()?.ToList() ?? new List<DBTransport>();
        }

        [HttpGet("GetAllOptions")]
        public List<TransportOption> GetAllOptions()
        {

            return _dBTransportOptionService.GetAll().Select(x => _converter.GetTransportOptionFromDBTransportOption(x)).ToList() ?? new List<TransportOption>();
        }

        [HttpGet("GetNotifications")]
        public List<TransportReservation> GetNotifications()
        {
            DBTransportReservation[] reservations = _dBReservationService.GetAll().FindAll(x => !x.Paid).ToArray();
            return reservations.Select(x => _converter.GetTransportReservationFromDBTransportReservation(x)).ToList() ?? new List<TransportReservation>();
        }

        [HttpPost("ConfirmPayment")]
        public List<TransportReservation> ConfirmPayment(int reservationID)
        {
            DBTransportReservation reservation = _dBReservationService.GetById(reservationID);
            reservation.Paid = true;
            _dBReservationService.Update(reservation);
            DBTransportReservation[] reservations = _dBReservationService.GetAll().FindAll(x => !x.Paid).ToArray();
            return reservations.Select(x => _converter.GetTransportReservationFromDBTransportReservation(x)).ToList() ?? new List<TransportReservation>();
        }

        [HttpPost("AddOrUpdateOption")]
        public List<TransportOption> AddOrUpdateOption([FromBody] TransportOption transportOption)
        {
            DBCompany? company = _dBCompanyService.GetById(transportOption.Company.Id);
            DBTransport? transport = _dBTransportService.GetById(transportOption.Transport.Id);
            DBTransportOption? option = _dBTransportOptionService.GetById(transportOption.Id);

            if (company == null)
            {
                company = _dBCompanyService.Create(new DBCompany 
                { 
                    Name = transportOption.Company.Name 
                });
                transportOption.Company.Id = company?.Id;
            }

            if (transport == null)
            {
                transport = _dBTransportService.Create(new DBTransport
                {
                    SeatsCount = transportOption.Transport.SeatsCount,
                    Name = transportOption.Transport.Name,
                    TypeId = transportOption.Transport.TypeId
                });
                transportOption.Transport.Id = transport?.Id;
            }

            if (option == null)
            {
                DBTransportOption newOption = _converter.GetDBTransportOptionFromTransportOption(transportOption);
                newOption.SeatsLeft = transport.SeatsCount ?? 0;
                _dBTransportOptionService.Create(newOption);

            } else
            {
                if (transportOption.DepartureDate != option.DepartureDate || transportOption.ArrivalDate != option.ArrivalDate)
                {
                    List<DBTransportOptionTransportReservationRelation> relations = _dBTransportOptionTransportReservationRelationService.GetByTransportOptionID(transportOption.Id);
                    List<DBTransportReservation> reservations = relations.Select(x => _dBReservationService.GetById(x.TransportReservationId)).ToList();
                    List<DBUser> users = reservations.Select(x => _dBUserService.GetById(x.UserId)).ToList();
                    List<string> emailsSent = new List<string>();
                    foreach (DBUser user in users)
                    {
                        if (user.DelayNotification && !emailsSent.Contains(user.Email))
                        {
                            emailsSent.Add(user.Email);
                            _emailMessageSender.SendNotificationMessage(user.Email, _converter.GetDBTransportOptionFromTransportOption(transportOption));
                        }
                    }
                }
                _dBTransportOptionService.Update(_converter.GetDBTransportOptionFromTransportOption(transportOption));
            }

            return _dBTransportOptionService.GetAll().Select(x => _converter.GetTransportOptionFromDBTransportOption(x)).ToList() ?? new List<TransportOption>();
        }

        [HttpPost("DeleteOption")]
        public List<TransportOption> DeleteOption(int optionID)
        {
            _dBTransportOptionService.Delete(optionID);
            return _dBTransportOptionService.GetAll().Select(x => _converter.GetTransportOptionFromDBTransportOption(x)).ToList() ?? new List<TransportOption>();
        }
    }
}