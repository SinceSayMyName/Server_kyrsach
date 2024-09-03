using Microsoft.AspNetCore.Mvc;
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
    public class SearchController : ControllerBase
    {

        private readonly ILogger<SearchController> _logger;
        private readonly Converter _converter;
        private DBTransportOptionService _dBTransportOptionService;
        private DBUserService _dBUserService;
        private DBTransportReservationService _dBTransportReservationService;
        private DBTransportOptionTransportReservationRelationService _dBTransportOptionTransportReservationRelationService;
        private EmailMessageSender _emailMessageSender;

        public SearchController(ILogger<SearchController> logger, Converter converter, DBApplicationContext dBApplicationContext)
        {
            _logger = logger;
            _converter = converter;
            _dBTransportOptionService = new DBTransportOptionService(dBApplicationContext);
            _dBUserService = new DBUserService(dBApplicationContext);
            _dBTransportReservationService = new DBTransportReservationService(dBApplicationContext);
            _dBTransportOptionTransportReservationRelationService = new DBTransportOptionTransportReservationRelationService(dBApplicationContext);

            _emailMessageSender = new EmailMessageSender(dBApplicationContext, converter);
        }

        [HttpPost("GetSearchOptions")]
        public TransportOptionReturn GetSearchOptions([FromBody] SearchData searchData)
        {
            List< List<DBTransportOption>>? optionsTo = _dBTransportOptionService.GetTransportOptions(
                searchData.DepartureDate,
                searchData.DepartureCityCode,
                searchData.ArrivalCityCode,
                searchData.TransportTypeID,
                searchData.SeatsNumber
            );
            List< List<DBTransportOption>>? optionsFrom = _dBTransportOptionService.GetTransportOptions(
                searchData.ArrivalDate,
                searchData.ArrivalCityCode,
                searchData.DepartureCityCode,
                searchData.TransportTypeID,
                searchData.SeatsNumber
            );

            TransportOptionReturn options = new TransportOptionReturn
            {
                transportOptionTo = optionsTo.Select(x => x.Select(y => _converter.GetTransportOptionFromDBTransportOption(y)).ToList()).ToList() ?? new List<List<TransportOption>>(),
                transportOptionFrom = optionsFrom.Select(x => x.Select(y => _converter.GetTransportOptionFromDBTransportOption(y)).ToList()).ToList() ?? new List<List<TransportOption>>()
            };

            return options;
        }

        [HttpPost("SelectOption")]
        public TransportReservation SelectOption(int userID, int optionID, int passengerCount, bool luggageRequired, string? userEmail)
        {
            DBTransportReservation dBTransportReservation = new DBTransportReservation
            {
                PassengerCount = passengerCount,
                User = _dBUserService.GetById(userID),
                Paid = false,
                LuggageRequired = luggageRequired,
                UserEmail = userEmail
            };
            dBTransportReservation = _dBTransportReservationService.Create(dBTransportReservation);
            DBTransportOptionTransportReservationRelation relation = new DBTransportOptionTransportReservationRelation
            {
                TransportOption = _dBTransportOptionService.GetById(optionID),
                TransportReservation = _dBTransportReservationService.GetById(dBTransportReservation.Id)
            };
            _dBTransportOptionTransportReservationRelationService.Create(relation);
            _emailMessageSender.SendPaymentMessage(userEmail, dBTransportReservation);
            return _converter.GetTransportReservationFromDBTransportReservation(dBTransportReservation);
        }

        [HttpPost("ConfirmOptionPayment")]
        public TransportReservation ConfirmOptionPayment(int reservationID)
        {
            DBTransportReservation dBTransportReservation = _dBTransportReservationService.GetById(reservationID);
            dBTransportReservation.Paid = true;
            return _converter.GetTransportReservationFromDBTransportReservation(_dBTransportReservationService.Update(dBTransportReservation));
        }
    }
}