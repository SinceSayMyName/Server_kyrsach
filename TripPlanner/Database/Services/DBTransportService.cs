using TripPlanner.DBTripPlanner.Models;

namespace TripPlanner.DBTripPlanner.Services
{
    public class DBTransportService
    {
        DBApplicationContext _applicationContext;

        public DBTransportService(DBApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }


        public List<DBTransport>? GetAll()
        {
            return _applicationContext.DBTransportTable.ToList();
        }


        public DBTransport? GetById(int? id)
        {
            return _applicationContext.DBTransportTable.ToList().Find(x => x.Id == id);
        }

        public DBTransport? Create(DBTransport dBTransport)
        {
            _applicationContext.DBTransportTable.Add(dBTransport);
            _applicationContext.SaveChanges();
            return GetById(dBTransport.Id);
        }

        public DBTransport? Update(DBTransport dBTransport)
        {
            DBTransport transport = GetById(dBTransport.Id);
            transport.TypeId = dBTransport.Type.Id;
            transport.SeatsCount = dBTransport.SeatsCount;
            _applicationContext.SaveChanges();
            return GetById(dBTransport.Id);
        }
    }
}