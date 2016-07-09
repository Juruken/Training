using System.Collections.Generic;
using System.IO;

namespace TrainTrip.Providers
{
    public class DataProvider : IDataProvider
    {
        private readonly string m_FilePath;

        public DataProvider(string filePath)
        {
            m_FilePath = filePath;
        }

        /// <summary>
        /// Returns a list of string lines from the file. Does NOT do any validation on the file contents.
        /// </summary>
        /// <returns></returns>
        public List<string> GetData()
        {
            // TODO: Handle possible buffer overflow
            using (var reader = new StreamReader(m_FilePath))
            {
                var fileContents = new List<string>();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    fileContents.Add(line);
                }

                return fileContents;
            }
        }
    }
}
