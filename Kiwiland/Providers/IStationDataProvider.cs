using System.Collections.Generic;

namespace Kiwiland.Processors
{
    public interface IStationDataProvider
    {
        /// <summary>
        /// Returns a list of strings, each char is the initial of a station name e.g. "A"
        /// </summary>
        /// <returns></returns>
        List<string> GetData();
    }
}
