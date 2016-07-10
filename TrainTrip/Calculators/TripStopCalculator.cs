using System;
using System.Collections.Generic;
using TrainTrip.DataModel;
using TrainTrip.Exceptions;
using TrainTrip.Processors;

namespace TrainTrip.Calculators
{
    public class TripStopCalculator : ITripStopCalculator
    {
        private readonly IStationProvider m_StationProvider;
        private readonly Dictionary<Tuple<string, string>, List<Trip>> m_TripsBySourceAndDestination;

        public TripStopCalculator(IStationProvider stationProvider)
        {
            m_StationProvider = stationProvider;
            m_TripsBySourceAndDestination = new Dictionary<Tuple<string, string>, List<Trip>>();
        }

        
        public List<Trip> GetTripsByStops(string sourceStation, string destinationStation, int maximumStops)
        {
            ValidateStationsExist(sourceStation, destinationStation);

            var sourceDestinationKey = new Tuple<string, string>(sourceStation, destinationStation);

            if (m_TripsBySourceAndDestination.ContainsKey(sourceDestinationKey))
                return m_TripsBySourceAndDestination[sourceDestinationKey];

            return CalculateTripsByStops(sourceStation, destinationStation, maximumStops);
        }

        private List<Trip> CalculateTripsByStops(string sourceStation, string destinationStation, int maximumStops)
        {
            List<Trip> trips = null;
            var sourceDestinationKey = new Tuple<string, string>(sourceStation, destinationStation);
            var source = m_StationProvider.GetStation(sourceStation);

            foreach (var route in source.Routes.Values)
            {
                Trip trip;
                if (route.DestinationStation == destinationStation)
                {
                    trip = route.ConvertToTrip();
                }
                else
                {
                    trip = GenerateTrip(route.ConvertToTrip(), route.Distance, sourceStation, destinationStation, maximumStops);

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

            m_TripsBySourceAndDestination.Add(sourceDestinationKey, trips);

            return trips;
        }

        private Trip GenerateTrip(Trip currentTrip, int currentDistance, string sourceStaiton, string destinationStation, int maximumStops)
        {
            if (currentTrip.TotalStops >= maximumStops)
                return null;

            var source = m_StationProvider.GetStation(sourceStaiton);
            var destination = m_StationProvider.GetStation(destinationStation);

            if (source.Routes.ContainsKey(destination.Name)
                && source.Routes[destination.Name].ConvertToTrip().TotalStops < maximumStops)
            {
                var trip = new Trip
                {
                    TotalDistance = currentDistance + source.Routes[destination.Name].Distance,
                    TripName = sourceStaiton + source.Routes[destination.Name].DestinationStation
                };

                return trip;
            }

            foreach (var route in source.Routes.Values)
            {
                var potentialTrip = GenerateTrip(currentTrip, currentDistance + route.Distance, route.DestinationStation, destinationStation, maximumStops);

                if (potentialTrip == null || potentialTrip.TotalStops >= maximumStops)
                    continue;

                potentialTrip.TotalDistance += currentDistance;
                potentialTrip.TripName = sourceStaiton + potentialTrip.TripName;

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
