using System.Collections.Generic;
using System.Linq;
using TrainTrip.DataModel;
using TrainTrip.Exceptions;

namespace TrainTrip.Processors
{
    /// <summary>
    /// Responsible for calculating trips for a given source and destination location
    /// </summary>
    // TODO: Refactor all Calculators to implement a BaseTripCalculator, so we have a protected m_StationProvider
    // TODO: and a single validate stations exist function
    public class TripDirectRouteDistanceCalculator : ITripDirectRouteDistanceCalculator
    {
        private readonly IStationProvider m_StationProvider;
        private readonly Dictionary<string, List<Trip>> m_CalculatedTrips;

        public TripDirectRouteDistanceCalculator(IStationProvider stationProvider)
        {
            m_StationProvider = stationProvider;
            m_CalculatedTrips = new Dictionary<string, List<Trip>>();
        }

        public Trip GetDirectRouteByLowestDistance(string sourceStation, string destinationStation)
        {
            ValidateStationsExist(sourceStation, destinationStation);
            
            List<Trip> trips;
            var sourceDestinationKey = sourceStation + destinationStation;

            if (!m_CalculatedTrips.ContainsKey(sourceDestinationKey))
            {
                trips = GetDirectRoutesTripsByDistance(sourceStation, destinationStation);
            }
            else
            {
                trips = m_CalculatedTrips[sourceDestinationKey];
            }
            
            // Expecting the default to be null
            return trips != null ? trips.OrderBy(t => t.TotalDistance).FirstOrDefault() : null;
        }
        
        private List<Trip> GetDirectRoutesTripsByDistance(string sourceStation, string destinationStation)
        {
            ValidateStationsExist(sourceStation, destinationStation);

            var sourceDestinationKey = sourceStation + destinationStation;

            if (m_CalculatedTrips.ContainsKey(sourceDestinationKey))
                return m_CalculatedTrips[sourceDestinationKey];
            
            return CalculateTripsByDistance(sourceStation, destinationStation);
        }

        /// <summary>
        /// Calculates a trip from the source station to the destination station, under a given maximum distance.
        /// Allows for user to specify direct routes only. If true, trip will only return if there is a direct route from the sourceStation to destinationStation.
        /// e.g. 
        /// Given AB1, A -> B returns a route
        /// Given AC1, CB1, A -> B returns null
        /// </summary>
        /// <param name="sourceStation"></param>
        /// <param name="destinationStation"></param>
        /// <param name="maximumDistance"></param>
        /// <param name="directRoutesOnly"></param>
        /// <returns></returns>
        private List<Trip> CalculateTripsByDistance(string sourceStation, string destinationStation)
        {
            List<Trip> trips = null;
            var sourceDestinationKey = sourceStation + destinationStation;
            var source = m_StationProvider.GetStation(sourceStation);

            // Loop of each route to see if they can get to our required destination.
            foreach (var route in source.Routes.Values)
            {
                Trip trip = null;
                if (route.DestinationStation == destinationStation)
                {
                    trip = route.ConvertToTrip();
                }
                
                if (trip == null)
                    continue;

                if (trips == null)
                    trips = new List<Trip>();

                trips.Add(trip);
            }
            
            m_CalculatedTrips.Add(sourceDestinationKey, trips);

            return trips;
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
