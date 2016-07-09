using System.Collections.Generic;
using Kiwiland.Data;

namespace Kiwiland.Processors
{
    public class TripProcessor : ITripProcessor
    {
        private readonly IStationProvider m_StationProvider;

        public TripProcessor(IStationProvider stationProvider)
        {
            m_StationProvider = stationProvider;
        }

        public List<Trip> Process(string sourceStation, string destinationStation, int maximumDistance)
        {
            var source = m_StationProvider.GetStation(sourceStation);
            var destination = m_StationProvider.GetStation(destinationStation);

            var trip = GeneratorTrip(0, maximumDistance, source, destination);

            return trip;
        }

        internal List<Trip> GeneratorTrip(int currentDistance, int maximumDistance, Station currentStation, Station destinationStation)
        {
            return null;
        }
    }
}
