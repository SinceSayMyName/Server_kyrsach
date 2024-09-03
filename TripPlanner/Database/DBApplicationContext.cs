using Microsoft.EntityFrameworkCore;
using TripPlanner.Database.Models;
using TripPlanner.DBTripPlanner.Models;

namespace TripPlanner.DBTripPlanner
{
    public class DBApplicationContext : DbContext
    {
        public DbSet<DBTransportOption> DBTransportOptionTable { get; set; } = null!;
        public DbSet<DBTransportReservation> DBTransportReservationTable { get; set; } = null!;
        public DbSet<DBUser> DBUserTable { get; set; } = null!;
        public DbSet<DBCompany> DBCompanyTable { get; set; } = null!;
        public DbSet<DBTransport> DBTransportTable { get; set; } = null!;
        public DbSet<DBTransportType> DBTransportTypeTable { get; set; } = null!;
        public DbSet<DBCity> DBCityTable { get; set; } = null!;
        public DbSet<DBTransportOptionTransportReservationRelation> DBTransportOptionTransportReservationRelationTable { get; set; } = null!;
        

        public DBApplicationContext()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Constants.ConnectionString);
        }
    }
}
