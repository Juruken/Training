using System.Collections.Generic;
using TrainTrip.Data;

namespace TrainTrip.Calculators
{
    public interface ITripStopCalculator
    {
        Trip GetFastestTipByStops(string sourceStation, string destinationStation);
        List<Trip> GetTripsByStops(string sourceStation, string destinationStation, int maximumStops);
    }
}
