using System.Net;
using System.Net.Mail;
using TripPlanner.Controllers;
using TripPlanner.Controllers.Types;
using TripPlanner.Database.Services;
using TripPlanner.DBTripPlanner;
using TripPlanner.DBTripPlanner.Models;
using TripPlanner.DBTripPlanner.Services;

namespace TripPlanner.EMailNotifications
{
    public class EmailMessageSender
    {
        private readonly Converter _converter;
        private DBTransportOptionService _dBTransportOptionService;
        private DBCityService _dBCityService;
        private DBTransportOptionTransportReservationRelationService _dBTransportOptionTransportReservationRelationService;
        private SmtpClient smtpClient;


        public EmailMessageSender(DBApplicationContext dBApplicationContext, Converter converter)
        {
            _converter = converter;
            _dBCityService = new DBCityService(dBApplicationContext);
            _dBTransportOptionService = new DBTransportOptionService(dBApplicationContext);
            _dBTransportOptionTransportReservationRelationService = new DBTransportOptionTransportReservationRelationService(dBApplicationContext);

            smtpClient = new SmtpClient(Constants.SmtpClient)
            {
                Port = 587,
                Credentials = new NetworkCredential(Constants.Email, Constants.Password),
                EnableSsl = true,
            };
        }

        public void SendPaymentMessage(string email, DBTransportReservation reservation)
        {
            int[] optionsID = _dBTransportOptionTransportReservationRelationService.GetByTransportReservationID(reservation.Id).Select(x => x.TransportOptionId).ToArray();
            TransportOption[] options = optionsID.Select(x => _converter.GetTransportOptionFromDBTransportOption(_dBTransportOptionService.GetById(x))).ToArray();

            string message = "";
            double price = 0;
            foreach (var option in options)
            {
                string departureCity = _dBCityService.GetByCode(option.DepartureCityCode).Name;
                string arrivalCity = _dBCityService.GetByCode(option.ArrivalCityCode).Name;
                message += "Город отправления: " + departureCity + "<br/>" +
                    "Город прибытия: " + arrivalCity + "<br/>";
                price += reservation.LuggageRequired ? option.PriceWithLuggage : option.Price;
            }
            message += "Кол-во пассажиров: " + reservation.PassengerCount + "<br/>" +
                "Багаж: " + (reservation.LuggageRequired ? "включен" : "не включен") +
            "<br/>Стоимость: " + price * reservation.PassengerCount;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(Constants.Email),
                Subject = "Информация о поездке",
                Body = message,
                IsBodyHtml = true,
            };
            try
            {
                mailMessage.To.Add(email);
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void SendNotificationMessage(string email, DBTransportOption option)
        {
            string message = "Перенесен рейс:\n";

            string departureCity = _dBCityService.GetByCode(option.DepartureCityFiasCode).Name;
            string arrivalCity = _dBCityService.GetByCode(option.ArrivalCityFiasCode).Name;
            message += "Город отправления: " + departureCity + "<br/>" +
                "Город прибытия: " + arrivalCity + "<br/>";
            message += "Новая дата отправления: " + option.DepartureDate.ToString() + "<br/>";
            message += "Новая дата прибытия: " + option.ArrivalDate.ToString();

            var mailMessage = new MailMessage
            {
                From = new MailAddress(Constants.Email),
                Subject = "Перенос рейса",
                Body = message,
                IsBodyHtml = true,
            };
            try
            {
                mailMessage.To.Add(email);
                smtpClient.Send(mailMessage);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
