﻿using System.Collections.Generic;
using Kiwiland.Calculators;
using Kiwiland.Data;
using Kiwiland.Exceptions;
using Kiwiland.Processors;

namespace Kiwiland.Managers
{
    public class TripManager : ITripManager
    {
        private readonly ITripDistanceCalculator m_TripDistanceCalculator;
        private readonly ITripStopCalculator m_TripStopCalculator;
        private readonly ITripPermutationsCalculator m_TripPermutationsCalculator;
        private readonly IJourneyCalculator m_JourneyCalculator;
        
        public TripManager(ITripDistanceCalculator tripDistanceCalculator, ITripStopCalculator tripStopCalculator,
            ITripPermutationsCalculator tripPermutationsCalculator, IJourneyCalculator journeyCalculator)
        {
            m_TripDistanceCalculator = tripDistanceCalculator;
            m_TripStopCalculator = tripStopCalculator;
            m_TripPermutationsCalculator = tripPermutationsCalculator;
            m_JourneyCalculator = journeyCalculator;
        }

        public Trip GetFastestTripByDistance(string sourceStation, string destinationStation, int maximumDistance)
        {
            return m_TripDistanceCalculator.GetFastestTripByDistance(sourceStation, destinationStation);
        }

        public List<Trip> GetTripsByDistance(string sourceStation, string destinationStation, int maximumDistance)
        {
            return m_TripDistanceCalculator.GetTripsByDistance(sourceStation, destinationStation, maximumDistance);
        }

        public Trip GetFastestTripByStops(string sourceStation, string destinationStation)
        {
            return m_TripStopCalculator.GetFastestTipByStops(sourceStation, destinationStation);
        }

        public List<Trip> GetTripsByStops(string sourceStation, string destinationStation, int maximumStops)
        {
            return m_TripStopCalculator.GetTripsByStops(sourceStation, destinationStation, maximumStops);
        }
        
        public List<Trip> GetPermutations(string sourceStation, string destinationStation, int maximumDistance)
        {
            return m_TripPermutationsCalculator.GetPermutations(sourceStation, destinationStation, maximumDistance);
        }

        public Journey GetJourney(string[] stations)
        {
            return m_JourneyCalculator.Calculate(stations);
        }

        public int GetJourneyLengthByDistance(string[] stations)
        {
            var journey = GetJourney(stations);

            if (journey == null)
                throw new InvalidJourneyException(stations.ToString());

            return journey.Distance;
        }
    }
}
