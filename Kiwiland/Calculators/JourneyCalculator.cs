using System;
using System.Collections.Generic;
using Kiwiland.Data;
using Kiwiland.Processors;

namespace Kiwiland.Calculators
{
    public class JourneyCalculator : IJourneyCalculator
    {
        private readonly IStationProvider m_StationProvider;

        public JourneyCalculator(IStationProvider stationProvider)
        {
            m_StationProvider = stationProvider;
        }

        /// <summary>
        /// Accepts an array of Station Names. Will calculate a journey in index order, if any of the stations don't exist it will throw an argument exception.
        /// </summary>
        /// <param name="plannedJourney"></param>
        /// <returns></returns>
        public Journey Calculate(string[] plannedJourney)
        {
            ValidateStations(plannedJourney);

            return null;
        }

        private void ValidateStations(string[] stationNames)
        {
            foreach (var stationName in stationNames)
            {
                var station = m_StationProvider.GetStation(stationName);
                if (station == null)
                    throw new ArgumentException("Invalid Station Name.");
            }
        }
    }
}
