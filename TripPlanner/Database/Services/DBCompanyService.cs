using TripPlanner.DBTripPlanner.Models;

namespace TripPlanner.DBTripPlanner.Services
{
    public class DBCompanyService
    {
        DBApplicationContext _applicationContext;

        public DBCompanyService(DBApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public List<DBCompany>? GetAll()
        {
            return _applicationContext.DBCompanyTable.ToList();
        }

        public DBCompany? GetById(int? id)
        {
            return _applicationContext.DBCompanyTable.ToList().Find(x => x.Id == id);
        }

        public DBCompany? Create(DBCompany dBCompany)
        {
            _applicationContext.DBCompanyTable.Add(dBCompany);
            _applicationContext.SaveChanges();
            return GetById(dBCompany.Id);
        }

        public DBCompany? Update(DBCompany dBCompany)
        {
            DBCompany user = GetById(dBCompany.Id);
            user.Name = dBCompany.Name;
            _applicationContext.SaveChanges();
            return GetById(dBCompany.Id);
        }
    }
}
