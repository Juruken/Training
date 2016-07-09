using System.Collections.Generic;
using System.Linq;
using Kiwiland.Exceptions;
using Kiwiland.Validators;

namespace Kiwiland.Processors
{
    /// <summary>
    /// Responsible for processing raw station data. If the data is in an unexpected format it will throw an exception.
    /// </summary>
    public class StationDataProcessor : IStationDataProcessor
    {
        private readonly char m_Delimiter;
        private readonly IStationDataValidator m_StationDataValidator;

        public StationDataProcessor(IStationDataValidator validator, char delimiter)
        {
            m_Delimiter = delimiter;
            m_StationDataValidator = validator;
        }

        /// <summary>
        /// Expects a raw string of file lines, seperated by the injected delimeter.
        /// </summary>
        /// <returns>Processed Stations in their String format</returns>
        public List<string> Process(List<string> rawStationData)
        {
            var stations = new List<string>();

            foreach (var line in rawStationData)
            {
                var stationLine = line;
                if (m_Delimiter != ' ')
                {
                    stationLine = stationLine.Replace(" ", "");
                }

                var stationStrings = stationLine.Split(m_Delimiter);

                foreach (var station in stationStrings)
                {
                    if (!m_StationDataValidator.Validate(station))
                    {
                        throw new InvalidStationFormat(station);
                    }

                    stations.Add(station);
                }
            }

            return stations.ToList();
        }
    }
}
