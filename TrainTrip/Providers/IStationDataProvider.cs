using System.Collections.Generic;

namespace TrainTrip.Processors
{
    public interface IStationDataProvider
    {
        List<string> GetData();
    }
}
