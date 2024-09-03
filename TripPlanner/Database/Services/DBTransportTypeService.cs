using TripPlanner.DBTripPlanner.Models;

namespace TripPlanner.DBTripPlanner.Services
{
    public class DBTransportTypeService
    {
        DBApplicationContext _applicationContext;

        public DBTransportTypeService(DBApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }


        public List<DBTransportType>? GetAll()
        {
            return _applicationContext.DBTransportTypeTable.ToList();
        }


        public DBTransportType? GetById(int? id)
        {
            return _applicationContext.DBTransportTypeTable.ToList().Find(x => x.Id == id);
        }

        public DBTransportType? Create(DBTransportType dBTransportType)
        {
            _applicationContext.DBTransportTypeTable.Add(dBTransportType);
            _applicationContext.SaveChanges();
            return GetById(dBTransportType.Id);
        }

        public DBTransportType? Update(DBTransportType dBTransportType)
        {
            DBTransportType transportType = GetById(dBTransportType.Id);
            transportType.Name = dBTransportType.Name;
            _applicationContext.SaveChanges();
            return GetById(dBTransportType.Id);
        }
    }
}