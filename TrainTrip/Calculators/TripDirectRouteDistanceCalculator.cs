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

            var trips = new List<Trip>();
            if (recursiveSearch)
            {
                trips = FindStationInTreeRecursively(new Dictionary<string, List<Trip>>(), sourceStation, sourceStation, destinationStation);
            }
            else
            {
                var source = m_StationProvider.GetStation(sourceStation);
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
        private List<Trip> FindStationInTreeRecursively(Dictionary<string, List<Trip>> stationsAlreadyVisited, string currentStationName, string sourceStation, string destinationStation)
        {
            // If we've already visited return what we found last time.
            if (stationsAlreadyVisited.ContainsKey(currentStationName))
                return stationsAlreadyVisited[currentStationName];

            // Add our newly visited station to the stations visited map, so we don't infinitely recurse.
            var potentialTrips = new List<Trip>();
            stationsAlreadyVisited.Add(currentStationName, potentialTrips);

            var currentStation = m_StationProvider.GetStation(currentStationName);

            // Need to explore every possible route!
            foreach (var route in currentStation.Routes.Values)
            {
                if (route.DestinationStation == destinationStation)
                {
                    potentialTrips.Add(route.ConvertToTrip());
                    continue;
                }

                var resultingTrips = FindStationInTreeRecursively(stationsAlreadyVisited, route.DestinationStation, sourceStation, destinationStation);
                if (resultingTrips == null || !resultingTrips.Any())
                    continue;

                foreach (var tripResult in resultingTrips)
                {
                    var fullTrip = new Trip();
                    fullTrip.TotalDistance = route.Distance + tripResult.TotalDistance;
                    fullTrip.TripName = currentStation.Name + tripResult.TripName;

                    if (tripResult.TripName.EndsWith(destinationStation))
                    {
                        potentialTrips.Add(fullTrip);
                    }
                }
            }

            return potentialTrips;
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
