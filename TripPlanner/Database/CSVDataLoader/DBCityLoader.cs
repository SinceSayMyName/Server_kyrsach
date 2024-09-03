using System.Globalization;
using CsvHelper;
using TripPlanner.Database.CSVDataLoader;
using TripPlanner.DBTripPlanner.Models;
using TripPlanner.DBTripPlanner.Services;

namespace TripPlanner.DBTripPlanner.CSVDataLoader
{
    public class DBCityLoader
    {
        private DBCityService _dBCityService;

        public DBCityLoader(DBApplicationContext dBApplicationContext)
        {
            _dBCityService = new DBCityService(dBApplicationContext);
        }

        public void LoadIfEmpty()
        {
            if (_dBCityService.GetAll()?.Count == 0)
            {
                Load();
            };
        }

        public void Load()
        {
            using (var reader = new StreamReader("Data/city/city.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<CSVCity>();
                foreach (var record in records)
                {
                    string name = "";
                    if (record.region_type == "г")
                    {
                        name = record.region;
                    }
                    if (record.area_type == "г")
                    {
                        name = record.area;
                    }
                    if (record.city_type == "г")
                    {
                        name = record.city;
                    }
                    if (record.settlement_type == "г")
                    {
                        name = record.settlement;
                    }
                    DBCity city = new DBCity
                    {
                        Name = name,
                        FiasCode = record.fias_id
                    };
                    _dBCityService.Create(city);
                }
            }
        }
    }
}
