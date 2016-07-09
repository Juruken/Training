using System;
using System.Configuration;
using System.IO;
using TrainTrip.Factory;
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

            var inputDelimeterString = ConfigurationManager.AppSettings["InputFileDelimeter"];
            if (inputDelimeterString == null || inputDelimeterString.Length != 1)
                throw new ConfigurationErrorsException("Invalid Configuration, please specify character delimeter of input file.");
            
            var dataProvider = new DataProvider(filePath);
            var tripFactory = new TripFactory(inputDelimeterString[0], dataProvider);
            var tripManager = tripFactory.CreateTripManager();

            // TODO: Do Stuff
            

            // While input from console != Exit.

            // Fetch User Input

            // Validate User Input
            
            // Get Data for User Input

            // Output to Console

            Console.WriteLine("Success");
        }
    }
}
