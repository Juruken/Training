using System.Collections.Generic;
using System.Linq;
using TrainTrip.Calculators;
using TrainTrip.DataModel;
using TrainTrip.Processors;

namespace TrainTrip.Managers
{
    public class TripManager : ITripManager
    {
        private readonly ITripDirectRouteDistanceCalculator m_TripDirectRouteDistanceCalculator;
        private readonly ITripPermutationsCalculator m_TripStopPermutationsCalculator;
        private readonly ITripPermutationsCalculator m_TripDistancePermutationsCalculator;
        private readonly IJourneyCalculator m_JourneyCalculator;
        
        public TripManager(ITripDirectRouteDistanceCalculator tripDirectRouteDistanceCalculator, ITripPermutationsCalculator tripStopPermutationsCalculator,
            ITripPermutationsCalculator tripDistancePermutationsCalculator, IJourneyCalculator journeyCalculator)
        {
            m_TripDirectRouteDistanceCalculator = tripDirectRouteDistanceCalculator;
            m_TripStopPermutationsCalculator = tripStopPermutationsCalculator;
            m_TripDistancePermutationsCalculator = tripDistancePermutationsCalculator;
            m_JourneyCalculator = journeyCalculator;
        }

        public int? GetJourneyDistance(string[] stations)
        {
            var journey = m_JourneyCalculator.GetJourneyByRoutes(stations);
            return journey != null ? (int?)journey.Distance : null;
        }

        public int? GetShortestRouteByDistanceWithRecursion(string sourceStation, string destinationStation)
        {
            var trip = m_TripDirectRouteDistanceCalculator.GetDirectRouteByLowestDistanceWithRecursion(sourceStation, destinationStation);
            return trip != null ? (int?)trip.TotalDistance : null;
        }

        public int GetRoutesByMaximumStops(string sourceStation, string destinationStation, int maximumStops)
        {
            var trips = m_TripStopPermutationsCalculator.GetPermutations(sourceStation, destinationStation, maximumStops);
            return trips != null ? trips.Count : 0;
        }

        public int GetTripPermutationsCountByStops(string sourceStation, string destinationStation, int maximumStops)
        {
            var trips = m_TripStopPermutationsCalculator.GetPermutations(sourceStation, destinationStation, maximumStops);
            return trips != null ? trips.Count : 0;
        }

        public int GetExactTripPermutationsCountByStops(string sourceStation, string destinationStation, int exactStops)
        {
            var trips = m_TripStopPermutationsCalculator.GetPermutations(sourceStation, destinationStation, exactStops);
            return trips != null ? trips.Count(t => t.TotalStops == exactStops) : 0;
        }

        public int GetTripPermutationsCountByDistance(string sourceStation, string destinationStation, int maximumDistance)
        {
            var trips = m_TripDistancePermutationsCalculator.GetPermutations(sourceStation, destinationStation, maximumDistance);
            return trips != null ? trips.Count : 0;
        }
    }
}
