using System.Collections.Generic;
using TrainTrip.DataModel;

namespace TrainTrip.Calculators
{
    public interface ITripStopCalculator
    {
        List<Trip> GetTripsByStops(string sourceStation, string destinationStation, int maximumStops);
    }
}
