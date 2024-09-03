using TripPlanner.DBTripPlanner;
using TripPlanner.Database.Models;

namespace TripPlanner.Database.Services
{
    public class DBTransportOptionTransportReservationRelationService
    {
        DBApplicationContext _applicationContext;

        public DBTransportOptionTransportReservationRelationService(DBApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public List<DBTransportOptionTransportReservationRelation>? GetAll()
        {
            return _applicationContext.DBTransportOptionTransportReservationRelationTable.ToList();
        }

        public DBTransportOptionTransportReservationRelation? GetById(int? id)
        {
            return _applicationContext.DBTransportOptionTransportReservationRelationTable.ToList().Find(x => x.Id == id);
        }

        public List<DBTransportOptionTransportReservationRelation>? GetByTransportReservationID(int? transportReservationID)
        {
            return _applicationContext.DBTransportOptionTransportReservationRelationTable.ToList().FindAll(x => x.TransportReservationId == transportReservationID);
        }

        public List<DBTransportOptionTransportReservationRelation>? GetByTransportOptionID(int? transportOptionID)
        {
            return _applicationContext.DBTransportOptionTransportReservationRelationTable.ToList().FindAll(x => x.TransportOptionId == transportOptionID);
        }

        public DBTransportOptionTransportReservationRelation? Create(DBTransportOptionTransportReservationRelation dBTransportReservation)
        {
            _applicationContext.DBTransportOptionTransportReservationRelationTable.Add(dBTransportReservation);
            _applicationContext.SaveChanges();
            return GetById(dBTransportReservation.Id);
        }

        public DBTransportOptionTransportReservationRelation? Update(DBTransportOptionTransportReservationRelation dBTransportOptionTransportReservationRelation)
        {
            DBTransportOptionTransportReservationRelation transportReservation = GetById(dBTransportOptionTransportReservationRelation.Id);
            transportReservation.TransportReservation = dBTransportOptionTransportReservationRelation.TransportReservation;
            transportReservation.TransportOption = dBTransportOptionTransportReservationRelation.TransportOption;
            _applicationContext.SaveChanges();
            return GetById(dBTransportOptionTransportReservationRelation.Id);
        }
    }
}
