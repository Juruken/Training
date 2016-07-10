using System.Collections.Generic;
using TrainTrip.Calculators;
using TrainTrip.DataModel;
using TrainTrip.Exceptions;
using TrainTrip.Processors;

namespace TrainTrip.Managers
{
    public class TripManager : ITripManager
    {
        private readonly ITripDistanceCalculator m_TripDistanceCalculator;
        private readonly ITripStopCalculator m_TripStopCalculator;
        private readonly ITripPermutationsCalculator m_TripPermutationsCalculator;
        private readonly IJourneyCalculator m_JourneyCalculator;
        
        public TripManager(ITripDistanceCalculator tripDistanceCalculator, ITripStopCalculator tripStopCalculator,
            ITripPermutationsCalculator tripPermutationsCalculator, IJourneyCalculator journeyCalculator)
        {
            m_TripDistanceCalculator = tripDistanceCalculator;
            m_TripStopCalculator = tripStopCalculator;
            m_TripPermutationsCalculator = tripPermutationsCalculator;
            m_JourneyCalculator = journeyCalculator;
        }

        public Trip GetFastestTripByDistance(string sourceStation, string destinationStation, int maximumDistance, bool directRouteOnly)
        {
            return m_TripDistanceCalculator.GetFastestTripByDistance(sourceStation, destinationStation, maximumDistance, directRouteOnly);
        }

        public List<Trip> GetTripsByDistance(string sourceStation, string destinationStation, int maximumDistance, bool directRouteOnly)
        {
            return m_TripDistanceCalculator.GetTripsByDistance(sourceStation, destinationStation, maximumDistance, directRouteOnly);
        }


        public List<Trip> GetTripsByStops(string sourceStation, string destinationStation, int maximumStops)
        {
            return m_TripStopCalculator.GetTripsByStops(sourceStation, destinationStation, maximumStops);
        }
        
        public List<Trip> GetPermutations(string sourceStation, string destinationStation, int maximumDistance)
        {
            return m_TripPermutationsCalculator.GetPermutations(sourceStation, destinationStation, maximumDistance);
        }

        public Journey GetJourney(string[] stations, int maximumDistance, bool directRouteOnly)
        {
            return m_JourneyCalculator.GetJourneyByRoutes(stations, maximumDistance, directRouteOnly);
        }
    }
}
