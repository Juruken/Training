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
            var trips = new List<Trip>();
            var sourceDestinationKey = new Tuple<string, string>(sourceStation, destinationStation);
            var source = m_StationProvider.GetStation(sourceStation);

            // Loop of each route to see if they can get to our required destination.
            foreach (var route in source.Routes.Values)
            {
                var generatedTrip = GeneratorTrip(route.Distance, maximumDistance, route.DestinationStation, destinationStation);

                if (generatedTrip == null)
                    continue;

                generatedTrip.TripName = sourceStation + generatedTrip.TripName;

                trips.Add(generatedTrip);
            }

            m_TripsBySourceAndDestination.Add(sourceDestinationKey, trips);
            
            return trips;
        }

        // Pass in CDC-16, CEBC-11
        // Expected Output CDC, CEBC, CEBCEBC

        // Starting from C we can follow EBC to get back to C. 
        // Starting from C we can follow DC to get back to C. 

        // Therefore we can go from C DC + DC 

        // BC4
        // CD8, CE2
        // DC8, DE6
        // EB3 

        // CDEBC
        // C-D 8, DE 14, EB 17, BC 21
        // C-D-E-B-C 21 
        internal Trip GeneratorTrip(int currentDistance, int maximumDistance, string currentStation, string destinationStation)
        {
            if (currentDistance > maximumDistance)
                return null;

            var current = m_StationProvider.GetStation(currentStation);
            
            foreach (var route in current.Routes.Values)
            {
                var potentialTrip = GeneratorTrip(currentDistance + route.Distance, maximumDistance, route.DestinationStation, destinationStation);

                if (potentialTrip == null || potentialTrip.TotalDistance >= maximumDistance)
                    continue;

                // Current permutation doesn't end at our destination
                if (!potentialTrip.TripName.EndsWith(destinationStation))
                    return null;
                
                potentialTrip.TotalDistance += currentDistance;
                potentialTrip.TripName = currentStation + potentialTrip.TripName;

                return potentialTrip;
            }

            return null;
        }

        // TODO: In order to do this, we need to traverse the tree until we hit the cap EVEN if... we have already reached our destination.
        // TODO: Have a flag to continue past the destination if maximum isn't reached?

        // TODO: Re-write data model

        // Station: List<Station> LinkedStations // Track Distance in Dictionary?
        // Station: Dictionary<string, int> m_StationDistanceByName;
    }
}
