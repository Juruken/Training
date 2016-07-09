using System.Collections.Generic;
using Kiwiland.Data;

namespace Kiwiland.Calculators
{
    public interface ITripStopCalculator
    {
        Trip GetFastestTipByStops(string sourceStation, string destinationStation);
        List<Trip> GetTripsByStops(string sourceStation, string destinationStation, int maximumStops);
    }
}
