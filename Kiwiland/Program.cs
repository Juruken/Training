using System;
using System.Configuration;
using System.IO;
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
                throw new Exception("Invalid Input File Path: " + filePath);

            var inputDelimeter = ConfigurationManager.AppSettings["InputFileDelimeter"];
            if (inputDelimeter == null || inputDelimeter.Length != 1)
                throw new ConfigurationErrorsException("Invalid configuration, please specify character delimeter of input file.");
            
            var fileProvider = new FileDataDataProvider(filePath);
            var fileData = fileProvider.GetFileData();
            
            // TODO: Add this to a Factory
            var routeDataValidator = new RouteDataValidator();
            var routeDataProcessor = new RouteDataProcessor(routeDataValidator, inputDelimeter[0]);
            var routeDataProvider = new RouteDataProvider(routeDataProcessor, fileData);
            var routeProvider = new RouteProvider(routeDataProvider);

            var stationDataValidator = new StationDataValidator();
            var stationDataProcessor = new StationDataProcessor(stationDataValidator, inputDelimeter[0]);
            var stationDataProvider = new StationDataProvider(stationDataProcessor, fileData);

            var stationProvider = new StationProvider(stationDataProvider, routeProvider);

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
