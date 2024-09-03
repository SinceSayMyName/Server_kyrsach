using TripPlanner.DBTripPlanner.Models;

namespace TripPlanner.DBTripPlanner.Services
{
    public class DBTransportReservationService
    {
        DBApplicationContext _applicationContext;

        public DBTransportReservationService(DBApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public List<DBTransportReservation>? GetAll()
        {
            return _applicationContext.DBTransportReservationTable.ToList();
        }

        public DBTransportReservation? GetById(int? id)
        {
            return _applicationContext.DBTransportReservationTable.ToList().Find(x => x.Id == id);
        }

        public List<DBTransportReservation>? GetByUserID(int? userID)
        {
            return _applicationContext.DBTransportReservationTable.ToList().FindAll(x => x.UserId == userID);
        }

        public DBTransportReservation? Create(DBTransportReservation dBTransportReservation)
        {
            _applicationContext.DBTransportReservationTable.Add(dBTransportReservation);
            _applicationContext.SaveChanges();
            return GetById(dBTransportReservation.Id);
        }

        public DBTransportReservation? Update(DBTransportReservation dBTransportReservation)
        {
            DBTransportReservation transportReservation = GetById(dBTransportReservation.Id);
            transportReservation.User = dBTransportReservation.User;
            transportReservation.PassengerCount = dBTransportReservation.PassengerCount;
            _applicationContext.SaveChanges();
            return GetById(dBTransportReservation.Id);
        }
    }
}