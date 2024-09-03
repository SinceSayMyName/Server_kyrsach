using TripPlanner.DBTripPlanner.Models;

namespace TripPlanner.DBTripPlanner.Services
{
    public class DBUserService
    {
        DBApplicationContext _applicationContext;

        public DBUserService(DBApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public DBUser? Authenticate(string login, string password)
        {
            return _applicationContext.DBUserTable.ToList().Find(x => x.Login == login && x.Password == password);
        }

        public List<DBUser>? GetAll()
        {
            return _applicationContext.DBUserTable.ToList();
        }

        public DBUser? GetById(int? id)
        {
            return _applicationContext.DBUserTable.ToList().Find(x => x.Id == id);
        }

        public DBUser? Create(DBUser dBUser)
        {
            _applicationContext.DBUserTable.Add(dBUser);
            _applicationContext.SaveChanges();
            return GetById(dBUser.Id);
        }

        public DBUser? Update(DBUser dBUser)
        {
            DBUser user = GetById(dBUser.Id);
            user.Login = dBUser.Login;
            user.Password = dBUser.Password;
            user.Phone = dBUser.Phone;
            user.DelayNotification = dBUser.DelayNotification;
            user.IsAdmin = dBUser.IsAdmin;
            user.Email = dBUser.Email;
            _applicationContext.SaveChanges();
            return GetById(dBUser.Id);
        }
    }
}
