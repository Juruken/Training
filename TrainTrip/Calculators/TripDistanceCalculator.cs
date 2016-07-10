using System;
using System.Collections.Generic;
using System.Linq;
using TrainTrip.Data;
using TrainTrip.Exceptions;

namespace TrainTrip.Processors
{
    /// <summary>
    /// Responsible for calculating trips for a given source and destination location
    /// </summary>
    public class TripDistanceCalculator : ITripDistanceCalculator
    {
        private readonly IStationProvider m_StationProvider;
        private readonly Dictionary<Tuple<string, string>, List<Trip>> m_CalculatedTrips;

        public TripDistanceCalculator(IStationProvider stationProvider)
        {
            m_StationProvider = stationProvider;
            m_CalculatedTrips = new Dictionary<Tuple<string, string>, List<Trip>>();
        }

        public Trip GetFastestTripByDistance(string sourceStation, string destinationStation, int maximumDistance, bool directRoutesOnly)
        {
            ValidateStationsExist(sourceStation, destinationStation);
            
            List<Trip> trips;
            var sourceDestinationKey = new Tuple<string, string>(sourceStation, destinationStation);

            if (!m_CalculatedTrips.ContainsKey(sourceDestinationKey))
            {
                trips = GetTripsByDistance(sourceStation, destinationStation, maximumDistance, directRoutesOnly);
            }
            else
            {
                trips = m_CalculatedTrips[sourceDestinationKey];
            }
            
            // Expecting the default to be null
            return trips != null ? trips.OrderBy(t => t.TotalDistance).FirstOrDefault() : null;
        }
        
        public List<Trip> GetTripsByDistance(string sourceStation, string destinationStation, int maximumDistance, bool directRoutesOnly)
        {
            ValidateStationsExist(sourceStation, destinationStation);
            
            var sourceDestinationKey = new Tuple<string, string>(sourceStation, destinationStation);

            if (m_CalculatedTrips.ContainsKey(sourceDestinationKey))
                return m_CalculatedTrips[sourceDestinationKey];
            
            return CalculateTripsByDistance(sourceStation, destinationStation, maximumDistance, directRoutesOnly);
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
        private List<Trip> CalculateTripsByDistance(string sourceStation, string destinationStation, int maximumDistance, bool directRoutesOnly)
        {
            List<Trip> trips = null;
            var sourceDestinationKey = new Tuple<string, string>(sourceStation, destinationStation);

            var source = m_StationProvider.GetStation(sourceStation);

            // Loop of each route to see if they can get to our required destination.
            foreach (var route in source.Routes.Values)
            {
                Trip trip = null;
                if (route.DestinationStation == destinationStation)
                {
                    trip = route.ConvertToTrip();
                }
                else if (!directRoutesOnly)
                {
                    trip = GenerateTrip(route.Distance, maximumDistance, route.DestinationStation, destinationStation);

                    if (trip == null)
                        continue;

                    trip.TripName = sourceStation + trip.TripName;
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

        private Trip GenerateTrip(int currentDistance, int maximumDistance, string currentStation, string destinationStation)
        {
            if (currentDistance > maximumDistance)
                return null;
            
            var current = m_StationProvider.GetStation(currentStation);
            var destination = m_StationProvider.GetStation(destinationStation);

            if (current.Routes.ContainsKey(destination.Name)
                && current.Routes[destination.Name].Distance + currentDistance < maximumDistance)
            {
                var trip = new Trip
                {
                    TotalDistance = currentDistance + current.Routes[destination.Name].Distance,
                    TripName = currentStation + current.Routes[destination.Name].DestinationStation
                };

                return trip;
            }

            foreach (var route in current.Routes.Values)
            {
                var potentialTrip = GenerateTrip(currentDistance + route.Distance, maximumDistance, route.DestinationStation, destinationStation);

                if (potentialTrip == null || potentialTrip.TotalDistance >= maximumDistance)
                    continue;

                potentialTrip.TotalDistance += currentDistance;
                potentialTrip.TripName = currentStation + potentialTrip.TripName;

                return potentialTrip;
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
