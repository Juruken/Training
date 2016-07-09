using System.Collections.Generic;
using System.Linq;
using Kiwiland.Data;
using Kiwiland.Processors;

namespace Kiwiland.Managers
{
    public class TripManager
    {
        private readonly ITripProcessor m_TripProcessor;
        
        public TripManager(ITripProcessor tripProcessor, IStationProvider stationProvider, IRouteProvider routeProvider)
        {
            m_TripProcessor = tripProcessor;
        }
        
        public Trip GetFastestTrip(string sourceStation, string destinationStation, int distanceLimit)
        {
            Trip fastestTrip = null;
            var generatedTrips = m_TripProcessor.Process(sourceStation, destinationStation, distanceLimit);

            if (generatedTrips == null)
            {
                return null;
            }
            
            foreach (var trip in generatedTrips)
            {
                if (fastestTrip == null)
                {
                    fastestTrip = trip;
                }
                else if (trip.TotalDistance < fastestTrip.TotalDistance)
                {
                    fastestTrip = trip;
                }
            }

            return fastestTrip;
        }
    }
}
