using System;
using System.Collections.Generic;
using System.Linq;
using TrainTrip.DataModel;
using TrainTrip.Exceptions;
using TrainTrip.Processors;

namespace TrainTrip.Calculators
{
    public class TripPermutationsCalculator : ITripPermutationsCalculator
    {
        private readonly IStationProvider m_StationProvider;
        private readonly Dictionary<Tuple<string,string>, List<Trip>> m_TripsBySourceAndDestination;
        
        public TripPermutationsCalculator(IStationProvider stationProvider)
        {
            m_StationProvider = stationProvider;
            m_TripsBySourceAndDestination = new Dictionary<Tuple<string, string>, List<Trip>>();
        }

        public List<Trip> GetPermutations(string sourceStation, string destinationStation, int maximumDistance)
        {
            var sourceDestinationKey = new Tuple<string, string>(sourceStation, destinationStation);

            if (m_TripsBySourceAndDestination.ContainsKey(sourceDestinationKey))
                return m_TripsBySourceAndDestination[sourceDestinationKey];

            return CalculatePermutations(sourceStation, destinationStation, maximumDistance);
        }

        private List<Trip> CalculatePermutations(string sourceStation, string destinationStation, int maximumDistance)
        {
            ValidateStationsExist(sourceStation, destinationStation);

            var sourceDestinationKey = new Tuple<string, string>(sourceStation, destinationStation);
            var source = m_StationProvider.GetStation(sourceStation);

            var tripsByTripName = new Dictionary<string, Trip>();

            foreach (var route in source.Routes.Values)
            {
                var trip = route.ConvertToTrip();

                GeneratorTrip(tripsByTripName, trip, maximumDistance, route.DestinationStation, destinationStation);
            }

            var result = tripsByTripName.Values.ToList();

            m_TripsBySourceAndDestination.Add(sourceDestinationKey, result);
            
            return result;
        }

        private void GeneratorTrip(Dictionary<string, Trip> tripsByTripName, Trip currentTrip, int maximumDistance, string currentStation, string destinationStation)
        {
            if (currentTrip.TotalDistance > maximumDistance)
                return;

            var current = m_StationProvider.GetStation(currentStation);
            
            foreach (var route in current.Routes.Values)
            {
                var newTrip = currentTrip.Clone();
                newTrip.TotalDistance += route.Distance;
                newTrip.TripName += route.DestinationStation;
                
                if (newTrip.TotalDistance >= maximumDistance)
                    continue;
                
                if (newTrip.TripName.EndsWith(destinationStation) && !tripsByTripName.ContainsKey(newTrip.TripName))
                    tripsByTripName.Add(newTrip.TripName, newTrip.Clone());

                GeneratorTrip(tripsByTripName, newTrip.Clone(), maximumDistance, route.DestinationStation, destinationStation);
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
