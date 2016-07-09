using System.Collections.Generic;
using Kiwiland.Data;
using Kiwiland.Processors;

namespace Kiwiland.Calculators
{
    public class TripStopCalculator : ITripStopCalculator
    {
        private readonly IStationProvider m_StationProvider;

        public TripStopCalculator(IStationProvider stationProvider)
        {
            m_StationProvider = stationProvider;
        }

        public Trip GetFastestTipByStops(string sourceStation, string destinationStation)
        {
            var source = m_StationProvider.GetStation(sourceStation);
            return null;
        }

        public List<Trip> GetTripsByStops(string sourceStation, string destinationStation, int maximumStops)
        {
            var source = m_StationProvider.GetStation(sourceStation);
            return null;
        }
    }
}
