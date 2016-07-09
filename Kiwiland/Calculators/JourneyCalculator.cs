using System;
using System.Collections.Generic;
using Kiwiland.Data;
using Kiwiland.Processors;

namespace Kiwiland.Calculators
{
    public class JourneyCalculator : IJourneyCalculator
    {
        private readonly IStationProvider m_StationProvider;
        private readonly ITripCalculator m_TripCalculator;

        public JourneyCalculator(IStationProvider stationProvider, ITripCalculator tripCalculator)
        {
            m_StationProvider = stationProvider;
            m_TripCalculator = tripCalculator;
        }

        /// <summary>
        /// Accepts an array of Station Names. Will calculate a journey in index order, if any of the stations don't exist it will throw an argument exception.
        /// </summary>
        /// <param name="plannedJourney"></param>
        /// <returns></returns>
        public Journey Calculate(string[] plannedJourney)
        {
            ValidatePlannedJourney(plannedJourney);

            string previousStation = null;
            var journey = new Journey()
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

                var trip = m_TripCalculator.GetShortestTrip(previousStation, stationName);
                
                if (trip == null)
                    throw new ArgumentException(String.Format("No trip for: {0}, {1}.", previousStation, stationName));

                journey.Trips.Add(trip);

                previousStation = stationName;
            }

            return journey;
        }

        private void ValidatePlannedJourney(string[] stationNames)
        {
            if (stationNames.Length == 1)
                throw new ArgumentException("Invalid journey plan.");

            foreach (var stationName in stationNames)
            {
                var station = m_StationProvider.GetStation(stationName);
                if (station == null)
                    throw new ArgumentException(String.Format("{0} is an invalid Station name.", stationName));
            }
        }
    }
}
