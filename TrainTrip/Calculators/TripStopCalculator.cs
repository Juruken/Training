using System.Collections.Generic;
using TrainTrip.Data;
using TrainTrip.Processors;

namespace TrainTrip.Calculators
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

            // TODO: 
            /*if (route.DestinationStation == destinationStation)
            {
                trip = route.ConvertToTrip();
            }
            else
            {
                trip = GenerateTrip(route.Distance, maximumDistance, route.DestinationStation, destinationStation);

                if (trip == null)
                    continue;

                trip.TripName = sourceStation + trip.TripName;
            }*/
            return null;
        }

        public List<Trip> GetTripsByStops(string sourceStation, string destinationStation, int maximumStops)
        {
            var source = m_StationProvider.GetStation(sourceStation);
            return null;
        }
    }
}
