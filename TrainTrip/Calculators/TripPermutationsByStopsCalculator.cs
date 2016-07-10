using System.Collections.Generic;
using System.Linq;
using TrainTrip.DataModel;
using TrainTrip.Exceptions;
using TrainTrip.Processors;

namespace TrainTrip.Calculators
{
    public class TripDistancePermutationsCalculator : ITripPermutationsCalculator
    {
        private readonly IStationProvider m_StationProvider;
        private readonly Dictionary<string, List<Trip>> m_TripsBySourceAndDestination;
        
        public TripDistancePermutationsCalculator(IStationProvider stationProvider)
        {
            m_StationProvider = stationProvider;
            m_TripsBySourceAndDestination = new Dictionary<string, List<Trip>>();
        }

        public List<Trip> GetPermutations(string sourceStation, string destinationStation, int maximum)
        {
            var sourceDestinationKey = sourceStation + destinationStation + maximum;

            if (m_TripsBySourceAndDestination.ContainsKey(sourceDestinationKey))
                return m_TripsBySourceAndDestination[sourceDestinationKey];

            return CalculatePermutations(sourceStation, destinationStation, maximum);
        }

        private List<Trip> CalculatePermutations(string sourceStation, string destinationStation, int maximum)
        {
            ValidateStationsExist(sourceStation, destinationStation);

            var sourceDestinationKey = sourceStation + destinationStation + maximum;
            var source = m_StationProvider.GetStation(sourceStation);

            var tripsByTripName = new Dictionary<string, Trip>();

            foreach (var route in source.Routes.Values)
            {
                var trip = route.ConvertToTrip();

                GeneratorTrip(tripsByTripName, trip, maximum, route.DestinationStation, destinationStation);
            }

            if (tripsByTripName.Values.Count == 0)
                return null;

            var result = tripsByTripName.Values.ToList();

            m_TripsBySourceAndDestination.Add(sourceDestinationKey, result);
            
            return result;
        }

        private void GeneratorTrip(Dictionary<string, Trip> tripsByTripName, Trip currentTrip, int maximum, string currentStation, string destinationStation)
        {
            if (currentTrip.TotalDistance > maximum)
                return;

            var current = m_StationProvider.GetStation(currentStation);
            
            foreach (var route in current.Routes.Values)
            {
                var newTrip = currentTrip.Clone();
                newTrip.TotalDistance += route.Distance;
                newTrip.TripName += route.DestinationStation;
                
                if (newTrip.TotalDistance >= maximum)
                    continue;
                
                if (newTrip.TripName.EndsWith(destinationStation) && !tripsByTripName.ContainsKey(newTrip.TripName))
                    tripsByTripName.Add(newTrip.TripName, newTrip.Clone());

                GeneratorTrip(tripsByTripName, newTrip.Clone(), maximum, route.DestinationStation, destinationStation);
            }
        }

        private void ValidateStationsExist(string sourceStation, string destinationStation)
        {
            var source = m_StationProvider.GetStation(sourceStation);
            if (source == null)
                throw new InvalidStationException(sourceStation);

            var destination = m_StationProvider.GetStation(destinationStation);
            if (destination == null)
                throw new InvalidStationException(destinationStation);
        }
    }
}
