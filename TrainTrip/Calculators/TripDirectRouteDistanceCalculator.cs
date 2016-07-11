using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using TrainTrip.DataModel;
using TrainTrip.Exceptions;

namespace TrainTrip.Processors
{
    /// <summary>
    /// Responsible for calculating trips for a given source and destination location
    /// </summary>
    // TODO: Refactor all Calculators to implement a BaseTripCalculator, so we have a protected m_StationProvider and a single validate stations exist function
    public class TripDirectRouteDistanceCalculator : ITripDirectRouteDistanceCalculator
    {
        private readonly IStationProvider m_StationProvider;
        private readonly Dictionary<string, List<Trip>> m_CalculatedTrips;

        public TripDirectRouteDistanceCalculator(IStationProvider stationProvider)
        {
            m_StationProvider = stationProvider;
            m_CalculatedTrips = new Dictionary<string, List<Trip>>();
        }

        public Trip GetDirectRouteByLowestDistanceWithoutRecursion(string sourceStation, string destinationStation)
        {
            ValidateStationsExist(sourceStation, destinationStation);

            List<Trip> trips;
            var sourceDestinationKey = sourceStation + destinationStation + false;

            if (!m_CalculatedTrips.ContainsKey(sourceDestinationKey))
            {
                trips = GetDirectRoutesTripsByDistance(sourceStation, destinationStation, false);
            }
            else
            {
                trips = m_CalculatedTrips[sourceDestinationKey];
            }

            // Expecting the default to be null
            return trips != null ? trips.OrderBy(t => t.TotalDistance).FirstOrDefault() : null;
        }

        public Trip GetDirectRouteByLowestDistanceWithRecursion(string sourceStation, string destinationStation)
        {
            ValidateStationsExist(sourceStation, destinationStation);
            
            List<Trip> trips;
            var sourceDestinationKey = sourceStation + destinationStation + true;

            if (!m_CalculatedTrips.ContainsKey(sourceDestinationKey))
            {
                trips = GetDirectRoutesTripsByDistance(sourceStation, destinationStation, true);
            }
            else
            {
                trips = m_CalculatedTrips[sourceDestinationKey];
            }
            
            // Expecting the default to be null
            return trips != null ? trips.OrderBy(t => t.TotalDistance).FirstOrDefault() : null;
        }
        
        private List<Trip> GetDirectRoutesTripsByDistance(string sourceStation, string destinationStation, bool recursiveSearch)
        {
            var sourceDestinationKey = sourceStation + destinationStation;

            if (m_CalculatedTrips.ContainsKey(sourceDestinationKey))
                return m_CalculatedTrips[sourceDestinationKey];

            var source = m_StationProvider.GetStation(sourceStation);

            var trips = new List<Trip>();
            if (recursiveSearch)
            {
                FindStationInTreeRecursively(trips, new HashSet<string>(), source, sourceStation, destinationStation);
            }
            else
            {
                FindStationInTreeWithoutRecursion(trips, new HashSet<string>(), source, destinationStation);
            }

            return trips;
        }

        private void FindStationInTreeWithoutRecursion(List<Trip> trips, HashSet<string> stationsAlreadyVisited, Station currentStation, string destinationStation)
        {
            if (stationsAlreadyVisited.Contains(currentStation.Name))
                return;

            stationsAlreadyVisited.Add(currentStation.Name);

            foreach (var route in currentStation.Routes.Values)
            {
                var newTrip = route.ConvertToTrip();

                if (route.DestinationStation == destinationStation)
                {
                    trips.Add(newTrip);
                }
            }
        }


        // Recursively explore tree until destination station is found. Do not explore the same station twice.
        private Trip FindStationInTreeRecursively(List<Trip> trips, HashSet<string> stationsAlreadyVisited, Station currentStation, string sourceStation, string destinationStation)
        {
            if (stationsAlreadyVisited.Contains(currentStation.Name))
                return null;

            stationsAlreadyVisited.Add(currentStation.Name);

            foreach (var route in currentStation.Routes.Values)
            {
                var newTrip = route.ConvertToTrip();

                if (route.DestinationStation == destinationStation)
                {
                    trips.Add(newTrip);
                    return newTrip;
                }
                
                var trip = FindStationInTreeRecursively(trips, stationsAlreadyVisited, m_StationProvider.GetStation(route.DestinationStation), sourceStation, destinationStation);
                if (trip == null)
                    continue;

                trip.TotalDistance = route.Distance + trip.TotalDistance;
                trip.TripName = currentStation.Name + trip.TripName;

                if (trip.TripName[0].ToString() != sourceStation)
                    return null;

                trips.Add(trip);
            }

            return null;
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
