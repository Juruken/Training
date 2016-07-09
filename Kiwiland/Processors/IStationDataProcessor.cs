using System.Collections.Generic;

namespace Kiwiland.Processors
{
    public interface IStationDataProcessor
    {
        List<string> Process(List<string> rawStationData);
    }
}
