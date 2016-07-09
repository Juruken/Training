using System.Collections.Generic;

namespace Kiwiland.Processors
{
    public interface IStationDataProcessor
    {
        /// <summary>
        /// Expects a raw string of files, seperated by the configured delimeter.
        /// </summary>
        /// <returns>Processed Stations in their String format</returns>
        List<string> Process(string rawStationData);
    }
}
