using System;
using System.Configuration;
using System.IO;
using Kiwiland.Calculators;
using Kiwiland.Processors;
using Kiwiland.Providers;
using Kiwiland.Validators;

namespace Kiwiland
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
            
            



            // Ready to get some stations!

            // TODO: Determine where to put core logic for finding locations.
            // TODO: Perhaps a StationProcessor or StationRouteProcessor

            // TODO: Still need to handle input and input
            
            // var stationManager = new StationManager(stations);
            // var routeManager = new RouteManager(stations);


            Console.WriteLine("Success");
        }
    }
}
