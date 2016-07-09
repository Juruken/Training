using System.Collections.Generic;

namespace Kiwiland.Processors
{
    public interface IStationDataProvider
    {
        List<string> GetData();
    }
}
