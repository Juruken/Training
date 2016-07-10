using System;
using System.Collections.Generic;
using TrainTrip.DataModel;
using TrainTrip.Exceptions;
using TrainTrip.Processors;

namespace TrainTrip.Calculators
{
    /// <summary>
    /// Calculates the distance of specified series of trips if they exist.
    /// e.g. A-B-C.
    /// </summary>
    public class JourneyCalculator : IJourneyCalculator
    {
        private readonly IStationProvider m_StationProvider;
        private readonly ITripDirectRouteDistanceCalculator m_TripDirectRouteDistanceCalculator;

        public JourneyCalculator(IStationProvider stationProvider, ITripDirectRouteDistanceCalculator tripDirectRouteDistanceCalculator)
        {
            m_StationProvider = stationProvider;
            m_TripDirectRouteDistanceCalculator = tripDirectRouteDistanceCalculator;
        }

        /// <summary>
        /// Accepts an array of Station Names. Will calculate a journey in index order, if any of the stations don't exist it will throw an argument exception.
        /// </summary>
        /// <param name="plannedJourney"></param>
        /// <param name="directRouteOnly"></param>
        /// <returns></returns>
        public Journey GetJourneyByRoutes(string[] plannedJourney)
        {
            ValidatePlannedJourney(plannedJourney);

            string previousStation = null;
            var journey = new Journey
            {
                Trips = new List<Trip>()
            };

            foreach (var stationName in plannedJourney)
            {
                if (previousStation == null)
                {
                    previousStation = stationName;
                    continue;
                }
                
                var trip = m_TripDirectRouteDistanceCalculator.GetDirectRouteByLowestDistance(previousStation, stationName);

                if (trip == null)
                {
                    throw new InvalidTripException(String.Format("No trip for: {0}, {1}.", previousStation, stationName));
                }

                journey.Trips.Add(trip);

                previousStation = stationName;
            }

            return journey;
        }

        private void ValidatePlannedJourney(string[] stationNames)
        {
            if (stationNames.Length == 1)
                throw new InvalidJourneyException("Invalid journey plan.");

            foreach (var stationName in stationNames)
            {
                var station = m_StationProvider.GetStation(stationName);
                if (station == null)
                    throw new InvalidStationException(String.Format("{0} is an invalid Station name.", stationName));
            }
        }
    }
}
