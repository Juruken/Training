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

        private readonly IFileDataProvider m_FileDataProvider;

        public TripFactory(char fileDelimiter, IFileDataProvider fileDataProvider)
        {
            m_FileDelimiter = fileDelimiter;
            m_FileDataProvider = fileDataProvider;
        }
        
        public ITripManager CreateTripManager()
        {
            var fileData = m_FileDataProvider.GetFileData();
            // TODO: Add a FileContents Validator? Isn't that what the stationDataProcessor and routeDataProcessors are for?!

            var routeDataValidator = new RouteDataValidator();
            var routeDataProcessor = new RouteDataProcessor(routeDataValidator, m_FileDelimiter);
            var routeDataProvider = new RouteDataProvider(routeDataProcessor, fileData);
            var routeProvider = new RouteProvider(routeDataProvider);

            var stationDataValidator = new StationDataValidator();
            var stationDataProcessor = new StationDataProcessor(stationDataValidator, m_FileDelimiter);
            var stationDataProvider = new StationDataProvider(stationDataProcessor, fileData);

            var stationProvider = new StationProvider(stationDataProvider, routeProvider);

            var tripDistanceCalculator = new TripDistanceCalculator(stationProvider);
            var tripPermutationsCalculator = new TripPermutationsCalculator(stationProvider);
            var journeyCalculator = new JourneyCalculator(stationProvider, tripDistanceCalculator);

            var tripStopCalculator = new TripStopCalculator(stationProvider);

            return new TripManager(tripDistanceCalculator, tripStopCalculator, tripPermutationsCalculator, journeyCalculator);
        }
    }
}
