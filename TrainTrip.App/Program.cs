using System;
using System.Configuration;
using System.IO;
using TrainTrip.Providers;

namespace TrainTrip
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = ConfigurationManager.AppSettings["InputFilePath"];
            
            if (!File.Exists(filePath))
                throw new FileLoadException("Invalid input File path", filePath);

            var inputDelimeter = ConfigurationManager.AppSettings["InputFileDelimeter"];
            if (inputDelimeter == null || inputDelimeter.Length != 1)
                throw new ConfigurationErrorsException("Invalid Configuration, please specify character delimeter of input file.");
            
            var fileProvider = new FileDataProvider(filePath);
            var fileData = fileProvider.GetFileData();
            
            

            Console.WriteLine("Success");
        }
    }
}
