using TripPlanner.DBTripPlanner.Models;

namespace TripPlanner.DBTripPlanner.Services
{
    public class DBCityService
    {
        DBApplicationContext _applicationContext;

        public DBCityService(DBApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }


        public List<DBCity>? GetAll()
        {
            return _applicationContext.DBCityTable.ToList();
        }

        public DBCity? GetByCode(string code)
        {
            return _applicationContext.DBCityTable.ToList().Find(x => x.FiasCode == code);
        }

        public DBCity? Create(DBCity dBCity)
        {
            _applicationContext.DBCityTable.Add(dBCity);
            _applicationContext.SaveChanges();
            return GetByCode(dBCity.FiasCode);
        }

        public DBCity? Update(DBCity dBCity)
        {
            DBCity transportType = GetByCode(dBCity.FiasCode);
            transportType.Name = dBCity.Name;
            _applicationContext.SaveChanges();
            return GetByCode(dBCity.FiasCode);
        }
    }
}