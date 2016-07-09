using System.Collections.Generic;

namespace TrainTrip.Providers
{
    public interface IRouteDataProcessor
    {
        List<string> Process(List<string> rawRouteData);
    }
}
