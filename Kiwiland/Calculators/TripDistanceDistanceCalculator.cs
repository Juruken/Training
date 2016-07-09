using System;
using System.Collections.Generic;
using System.Linq;
using Kiwiland.Data;
using Kiwiland.Exceptions;

namespace Kiwiland.Processors
{
    /// <summary>
    /// Responsible for calculating trips for a given source and destination location
    /// </summary>
    public class TripDistanceDistanceCalculator : ITripDistanceCalculator
    {
        // TODO: Make sure this doesn't eat up performance, pick a smaller default maximum?
        private const int DEFAULT_MAXIMUM_DISTANCE = 10000;

        private readonly IStationProvider m_StationProvider;
        private readonly Dictionary<Tuple<string, string>, List<Trip>> m_CalculatedTrips;

        public TripDistanceDistanceCalculator(IStationProvider stationProvider)
        {
            m_StationProvider = stationProvider;
            m_CalculatedTrips = new Dictionary<Tuple<string, string>, List<Trip>>();
        }

        public Trip GetFastestTripByDistance(string sourceStation, string destinationStation)
        {
            ValidateStationsExist(sourceStation, destinationStation);

            List<Trip> trips;
            var sourceDestinationKey = new Tuple<string, string>(sourceStation, destinationStation);

            if (!m_CalculatedTrips.ContainsKey(sourceDestinationKey))
            {
                trips = GetTripsByDistance(sourceStation, destinationStation);
            }
            else
            {
                trips = m_CalculatedTrips[sourceDestinationKey];
            }
            
            // Expecting the default to be null
            return trips.OrderBy(t => t.TotalDistance).FirstOrDefault();
        }

        // TODO: 

        public List<Trip> GetTripsByDistance(string sourceStation, string destinationStation, int maximumDistance = DEFAULT_MAXIMUM_DISTANCE)
        {
            ValidateStationsExist(sourceStation, destinationStation);

            var sourceDestinationKey = new Tuple<string, string>(sourceStation, destinationStation);

            if (m_CalculatedTrips.ContainsKey(sourceDestinationKey))
                return m_CalculatedTrips[sourceDestinationKey];
            
            return CalculateTripsByDistance(sourceStation, destinationStation, maximumDistance);
        }

        private List<Trip> CalculateTripsByDistance(string sourceStation, string destinationStation, int maximumDistance)
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

            m_CalculatedTrips.Add(sourceDestinationKey, trips);

            return trips;
        }

        private Trip GeneratorTrip(int currentDistance, int maximumDistance, string currentStation, string destinationStation)
        {
            if (currentDistance > maximumDistance)
                return null;
            
            var current = m_StationProvider.GetStation(currentStation);
            var destination = m_StationProvider.GetStation(destinationStation);

            if (current.Routes.ContainsKey(destination.Name) &&
                current.Routes[destination.Name].Distance + currentDistance < maximumDistance)
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
                var potentialTrip = GeneratorTrip(currentDistance + route.Distance, maximumDistance, route.DestinationStation, destinationStation);

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
