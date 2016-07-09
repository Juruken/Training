using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public List<string> Process(string rawStationData)
        {
            if (m_Delimiter != ' ')
            {
                rawStationData = rawStationData.Replace(" ", "");
            }

            var stationStrings = rawStationData.Split(m_Delimiter);

            foreach (var station in stationStrings)
            {
                if (!m_StationDataValidator.Validate(station))
                {
                    throw new ArgumentException("Station Data contains invalid format");
                }
            }

            return stationStrings.ToList();
        }
    }
}
