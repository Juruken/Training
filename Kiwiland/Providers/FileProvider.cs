using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Kiwiland.Providers
{
    public class FileProvider : IFileProvider
    {
        private readonly string m_FilePath;
        private readonly char m_InputDelimeter;

        public FileProvider(string filePath, char delimeter)
        {
            m_FilePath = filePath;
            m_InputDelimeter = delimeter;
        }

        public List<string> GetFileContents()
        {
            using (var reader = new StreamReader(m_FilePath))
            {
                var fileContents = new List<string>();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (m_InputDelimeter != ' ')
                    {
                        line = line.Replace(" ", "");
                    }

                    var tokens = line.Split(m_InputDelimeter).ToArray();
                    fileContents.AddRange(tokens);
                }

                return fileContents;
            }
        }
    }
}
