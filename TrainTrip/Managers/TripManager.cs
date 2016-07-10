using System.Collections.Generic;
using System.Linq;
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

        // TODO: Delete anything not being used

        public Trip GetShortestRouteByDistance(string sourceStation, string destinationStation, int maximumDistance, bool directRouteOnly)
        {
            return m_TripDistanceCalculator.GetFastestTripByDistance(sourceStation, destinationStation, maximumDistance, directRouteOnly);
        }

        public List<Trip> GetRoutesByDistance(string sourceStation, string destinationStation, int maximumDistance, bool directRouteOnly)
        {
            return m_TripDistanceCalculator.GetTripsByDistance(sourceStation, destinationStation, maximumDistance, directRouteOnly);
        }

        public List<Trip> GetRoutesByMaximumStops(string sourceStation, string destinationStation, int maximumStops)
        {
            return m_TripStopCalculator.GetTripsByStops(sourceStation, destinationStation, maximumStops);
        }

        public int GetCountOfRoutesForStationsByStops(string sourceStation, string destinationStation, int maximumStops)
        {
            var trips = m_TripStopCalculator.GetTripsByStops(sourceStation, destinationStation, maximumStops);
            return trips != null ? trips.Count : 0;
        }

        public int GetCountOfRoutesByExactStops(string sourceStation, string destinationStation, int exactStops)
        {
            var trips = m_TripStopCalculator.GetTripsByStops(sourceStation, destinationStation, exactStops);
            return trips != null ? trips.Count(t => t.TotalStops == exactStops) : 0;
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
