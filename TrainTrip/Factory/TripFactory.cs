using TrainTrip.Calculators;
using TrainTrip.Managers;
using TrainTrip.Processors;
using TrainTrip.Providers;
using TrainTrip.Validators;

namespace TrainTrip.Factory
{
    public class TripFactory
    {
        private readonly char m_FileDelimiter;

        private readonly IDataProvider m_DataProvider;

        public TripFactory(char fileDelimiter, IDataProvider dataProvider)
        {
            m_FileDelimiter = fileDelimiter;
            m_DataProvider = dataProvider;
        }
        
        public ITripManager CreateTripManager()
        {
            var routeDataValidator = new RouteDataValidator();
            var routeDataProcessor = new RouteDataProcessor(routeDataValidator, m_FileDelimiter);
            var routeDataProvider = new RouteDataProvider(m_DataProvider, routeDataProcessor);
            var routeProvider = new RouteProvider(routeDataProvider);

            var stationDataValidator = new StationDataValidator();
            var stationDataProcessor = new StationDataProcessor(stationDataValidator, m_FileDelimiter);
            var stationDataProvider = new StationDataProvider(m_DataProvider, stationDataProcessor);

            var stationProvider = new StationProvider(stationDataProvider, routeProvider);

            var tripDistanceCalculator = new TripDirectRouteDistanceCalculator(stationProvider);
            var tripPermutationsCalculator = new TripDistancePermutationsCalculator(stationProvider);
            var journeyCalculator = new JourneyCalculator(stationProvider, tripDistanceCalculator);

            var tripStopCalculator = new TripStopPermutationsCalculator(stationProvider);

            return new TripManager(tripDistanceCalculator, tripStopCalculator, tripPermutationsCalculator, journeyCalculator);
        }
    }
}
