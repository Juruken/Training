using System;
using System.Collections.Generic;
using System.Linq;
using Kiwiland.Data;
using Kiwiland.Processors;

namespace Kiwiland.Calculators
{
    public class TripPermutationsCalculator : ITripPermutationsCalculator
    {
        private readonly IStationProvider m_StationProvider;

        private Dictionary<Tuple<string,string>, List<Trip>> m_TripsBySourceAndDestination;
        
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

        internal List<Trip> CalculatePermutations(string sourceStation, string destinationStation, int maximumDistance)
        {
            ValidateStationsExist(sourceStation, destinationStation);

            var sourceDestinationKey = new Tuple<string, string>(sourceStation, destinationStation);
            var source = m_StationProvider.GetStation(sourceStation);

            var tripsByTripName = new Dictionary<string, Trip>();

            // Loop of each route to see if they can get to our required destination.
            foreach (var route in source.Routes.Values)
            {
                var currentTrip = new Trip
                {
                    TotalDistance = route.Distance,
                    TripName = sourceStation + route.DestinationStation
                };

                // TODO: DRY... fix this.
                var trips = GeneratorTrip(currentTrip, maximumDistance, route.DestinationStation, destinationStation);

                foreach (var trip in trips.Values)
                {
                    if (!tripsByTripName.ContainsKey(trip.TripName))
                        tripsByTripName.Add(trip.TripName, trip);
                }
            }

            var result = tripsByTripName.Values.ToList();

            m_TripsBySourceAndDestination.Add(sourceDestinationKey, result);
            
            return result;
        }

        internal Dictionary<string, Trip> GeneratorTrip(Trip currentTrip, int maximumDistance, string currentStation, string destinationStation)
        {
            if (currentTrip.TotalDistance > maximumDistance)
                return null;

            var tripsByTripName = new Dictionary<string, Trip>();
            var current = m_StationProvider.GetStation(currentStation);
            
            foreach (var route in current.Routes.Values)
            {
                currentTrip.TotalDistance += route.Distance;
                currentTrip.TripName += route.DestinationStation;
                
                if (currentTrip.TotalDistance >= maximumDistance)
                    continue;
                
                if (currentTrip.TripName.EndsWith(destinationStation) && !tripsByTripName.ContainsKey(currentTrip.TripName))
                    tripsByTripName.Add(currentTrip.TripName, currentTrip);

                var trips = GeneratorTrip(currentTrip, maximumDistance, route.DestinationStation, destinationStation);

                foreach (var trip in trips.Values)
                {
                    if (!tripsByTripName.ContainsKey(trip.TripName))
                        tripsByTripName.Add(trip.TripName, trip);
                }
            }

            return tripsByTripName;
        }

        internal void ValidateStationsExist(string sourceStation, string destinationStation)
        {
            var source = m_StationProvider.GetStation(sourceStation);
            if (source == null)
                throw new ArgumentException("Invalid Source Station");

            var destination = m_StationProvider.GetStation(destinationStation);
            if (destination == null)
                throw new ArgumentException("Invalid Destination Station");
        }
    }
}
