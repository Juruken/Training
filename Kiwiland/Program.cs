using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kiwiland.Providers;

namespace Kiwiland
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = ConfigurationManager.AppSettings["InputFilePath"];
            if (!File.Exists(filePath))
                throw new Exception("Invalid Input File Path: " + filePath);

            var inputDelimeter = ConfigurationManager.AppSettings["InputFileDelimeter"];
            if (inputDelimeter == null || inputDelimeter.Length != 1)
                throw new ConfigurationErrorsException("Invalid configuration, please specify character delimeter of input file.");
            
            var fileProvider = new FileDataDataProvider(filePath);
            var fileData = fileProvider.GetFileData();



            Console.WriteLine("Success");
        }
    }
}
