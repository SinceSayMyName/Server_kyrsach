using TripPlanner.DBTripPlanner.Models;

namespace TripPlanner.DBTripPlanner.Services
{
    public class DBTransportOptionService
    {
        DBApplicationContext _applicationContext;
        DBTransportService _dBTransportService;

        public DBTransportOptionService(DBApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            _dBTransportService = new DBTransportService(applicationContext);
        }

        public List<List<DBTransportOption>>? GetTransportOptions(DateTime date, string departureCityCode, string arrivalCityCode, int transportType, int seatsNumber)
        {
            //_applicationContext.DBTransportOptionTable.ToList().ForEach(x =>
            //{
            //    Console.WriteLine(x.ArrivalCityFiasCode + " - " + arrivalCityCode);
            //    Console.WriteLine(x.ArrivalCityFiasCode == arrivalCityCode);
            //    Console.WriteLine(x.DepartureCityFiasCode + " - " + departureCityCode);
            //    Console.WriteLine(x.DepartureCityFiasCode == departureCityCode);
            //    Console.WriteLine(x.DepartureDate.Date + " - " + date.ToLocalTime().Date);
            //    Console.WriteLine(x.DepartureDate.Date == date.ToLocalTime().Date);
            //    Console.WriteLine(_dBTransportService.GetById(x.TransportId).TypeId + " - " + transportType);
            //    Console.WriteLine(_dBTransportService.GetById(x.TransportId).TypeId == transportType);
            //    Console.WriteLine(x.SeatsLeft + " - " + seatsNumber);
            //    Console.WriteLine(x.SeatsLeft >= seatsNumber);
            //});

            List<DBTransportOption> departureOptions = _applicationContext.DBTransportOptionTable.ToList().FindAll(x =>
                x.DepartureCityFiasCode == departureCityCode &&
                x.DepartureDate.Date == date.ToLocalTime().Date &&
                x.SeatsLeft >= seatsNumber &&
                _dBTransportService.GetById(x.TransportId).TypeId == transportType
            );
            List<DBTransportOption> arrivalOptions = _applicationContext.DBTransportOptionTable.ToList().FindAll(x =>
                x.DepartureCityFiasCode != departureCityCode &&
                x.ArrivalCityFiasCode == arrivalCityCode &&
                x.DepartureDate >= date.ToLocalTime() &&
                x.SeatsLeft >= seatsNumber &&
                _dBTransportService.GetById(x.TransportId).TypeId == transportType
            );
            List<DBTransportOption> transferOptions = _applicationContext.DBTransportOptionTable.ToList().FindAll(x =>
                x.DepartureCityFiasCode != departureCityCode &&
                x.ArrivalCityFiasCode != arrivalCityCode &&
                x.DepartureDate >= date.ToLocalTime() &&
                x.SeatsLeft >= seatsNumber &&
                _dBTransportService.GetById(x.TransportId).TypeId == transportType
            );


            List<List<DBTransportOption>> allOptions = new List<List<DBTransportOption>>();
            List<List<DBTransportOption>> tempOptions = new List<List<DBTransportOption>>();

            foreach (DBTransportOption departureOption in departureOptions)
            {
                if (departureOption.ArrivalCityFiasCode == arrivalCityCode)
                {
                    allOptions.Add(new List<DBTransportOption> { departureOption });
                } else
                {
                    tempOptions.Add(new List<DBTransportOption> { departureOption });
                    foreach (DBTransportOption transferOption in transferOptions)
                    {
                        if (departureOption.ArrivalCityFiasCode == transferOption.DepartureCityFiasCode)
                        {
                            tempOptions.Add(new List<DBTransportOption> { departureOption, transferOption });
                        }
                    }
                }
            }
            foreach (DBTransportOption arrivalOption in arrivalOptions)
            {
                foreach (List<DBTransportOption> transferOption in tempOptions)
                {
                    if (arrivalOption.DepartureCity == transferOption[transferOption.Count - 1].ArrivalCity &&
                        arrivalOption.DepartureDate >= transferOption[transferOption.Count - 1].ArrivalDate)
                    {
                        transferOption.Add(arrivalOption);
                        allOptions.Add(transferOption);
                    }
                }
            }

            return allOptions;
        }

        public List<DBTransportOption>? GetAll()
        {
            return _applicationContext.DBTransportOptionTable.ToList();
        }

        public DBTransportOption? GetById(int? id)
        {
            return _applicationContext.DBTransportOptionTable.ToList().Find(x => x.Id == id);
        }

        public DBTransportOption? Create(DBTransportOption dBTransportOption)
        {
            dBTransportOption.DepartureDate = dBTransportOption.DepartureDate.ToLocalTime();
            dBTransportOption.ArrivalDate = dBTransportOption.ArrivalDate.ToLocalTime();
            _applicationContext.DBTransportOptionTable.Add(dBTransportOption);
            _applicationContext.SaveChanges();
            return GetById(dBTransportOption.Id);
        }

        public DBTransportOption? Update(DBTransportOption dBTransportOption)
        {
            DBTransportOption transportOption = GetById(dBTransportOption.Id);
            transportOption.ArrivalCity = dBTransportOption.ArrivalCity;
            transportOption.ArrivalDate = dBTransportOption.ArrivalDate.ToLocalTime();
            transportOption.DepartureCity = dBTransportOption.DepartureCity;
            transportOption.DepartureDate = dBTransportOption.DepartureDate.ToLocalTime();
            transportOption.TransportId = dBTransportOption.TransportId;
            transportOption.CompanyId = dBTransportOption.CompanyId;
            transportOption.Price = dBTransportOption.Price;
            transportOption.PriceWithLuggage = dBTransportOption.PriceWithLuggage;
            _applicationContext.SaveChanges();
            return GetById(dBTransportOption.Id);
        }

        public DBTransportOption? Delete(int dBTransportOptionID)
        {
            DBTransportOption option = GetById(dBTransportOptionID);
            if (option == null) return null;
            _applicationContext.DBTransportOptionTable.Remove(option);
            _applicationContext.SaveChanges();
            return option;
        }
    }
}
