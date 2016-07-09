using System.Collections.Generic;
using System.IO;

namespace Kiwiland.Providers
{
    public class FileDataProvider : IFileDataProvider
    {
        private readonly string m_FilePath;

        public FileDataProvider(string filePath)
        {
            m_FilePath = filePath;
        }

        /// <summary>
        /// Returns a list of string lines from the file. Does NOT do any validation on the file contents.
        /// </summary>
        /// <returns></returns>
        public List<string> GetFileData()
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
