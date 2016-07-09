using System.Collections.Generic;
using System.IO;

namespace Kiwiland.Providers
{
    public class FileDataDataProvider : IFileDataProvider
    {
        private readonly string m_FilePath;

        public FileDataDataProvider(string filePath)
        {
            m_FilePath = filePath;
        }

        /// <summary>
        /// Returns a list of string lines from the file
        /// </summary>
        /// <returns></returns>
        public List<string> GetFileData()
        {
            // TODO: Possible buffer overflow
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
