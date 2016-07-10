using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using TrainTrip.Exceptions;
using TrainTrip.Factory;
using TrainTrip.Managers;
using TrainTrip.Providers;

namespace TrainTrip.App
{
    class Program
    {       
        private enum InputType
        {
            InvalidInput,
            Help,
            Exit,
            GetCountOfTripsForStationsByStops,
            GetShorestRouteDistance,
            GetJourney,
            GetPermutations,
            GetTripsByDistance,
            GetTripsByMaximumStops,
            GetTripsByExactStops
        }

        private static string m_InitialUserPrompt;
        private static Dictionary<string, InputType> m_InputToOutput;
        private static Dictionary<InputType, string> m_PromptTextByInputType;

        // TODO: Write tests for this? 
        // TODO: Or better yet... Make this less bad!
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

            BuildOptions();
            BuildPromptText();
            BuildInitialUserPrompt();
            
            // While input from console != Exit.
            string input;

            Output(m_InitialUserPrompt);

            do
            {
                // Fetch User Input
                // Get option 1,2,3 etc.
                input = Console.ReadLine();
                
                // Validate User Input
                var inputType = ValidateInitialOptionInput(input);

                switch (inputType)
                {
                    case InputType.InvalidInput:
                        Output(m_PromptTextByInputType[InputType.InvalidInput]);
                        continue;
                    case InputType.Help:
                        Output(m_PromptTextByInputType[InputType.Help]);
                        continue;
                    case InputType.Exit:
                        Output(m_PromptTextByInputType[InputType.Exit]);
                        continue;
                }

                // Get Data for User Input
                var inputResult = ProcessInput(input, inputType);

                if (inputResult == InputType.InvalidInput)
                {
                    Output(m_PromptTextByInputType[InputType.Help]);
                    continue;
                }

                string result;
                try
                {
                    // If something is wrong with the input other than the format, we expect TrainTrip library to throw an exception.
                    result = Execute(input, inputType, tripManager);
                }
                catch (InvalidRouteException)
                {
                    result = "NO SUCH ROUTE";
                }
                catch (TrainTripException ex)
                {
                    result = ex.Message;
                }

                // Output to Console
                Output(result);
            } while (input != "Exit");
        }

        private static InputType ValidateInitialOptionInput(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
                return InputType.InvalidInput;

            if (m_InputToOutput.ContainsKey(inputString))
                return m_InputToOutput[inputString];

            // If we don't know what it is, assume it is invalid.
            return InputType.InvalidInput;
        }

        private static void Output(string output)
        {
            Console.WriteLine(output);
        }

        private static void BuildOptions()
        {
            m_InputToOutput = new Dictionary<string, InputType>
            {
                { "1", InputType.GetCountOfTripsForStationsByStops },
                { "2", InputType.GetShorestRouteDistance },
                { "3", InputType.GetJourney },
                { "4", InputType.GetPermutations },
                { "5", InputType.GetTripsByDistance },
                { "6", InputType.GetTripsByMaximumStops },
                { "7", InputType.GetTripsByExactStops },
                // TODO: Print available Routes
                // TODO: Print available Stations
                { "Help", InputType.Help },
                { "help", InputType.Help },
                { "Exit", InputType.Exit },
                { "exit", InputType.Exit }
            };
        }
        
        private static void BuildPromptText()
        {
            m_PromptTextByInputType = new Dictionary<InputType, string>
            {
                // The distance of the route <route> e.g. A-B-C
                // The number of trips starting at <StationName> end at <StationName> with a maximum of <Stops>
                { InputType.GetCountOfTripsForStationsByStops, "For the distance of the a route e.g. A-B-C, please enter the route <StationName>-<StationName> etc. (don't use < or >)." },
                // TODO: InputType.GetCountofTripsWithExactStops
                // The length of the shortest route (by distance) from <StationName> to <StationName>
                { InputType.GetShorestRouteDistance, "" },
                // The number of different routes from <StationName> to <StationName>
                { InputType.GetPermutations, "" },

                // The number of trips starting at <StationName> e.g. A ending at <StationName> e.g C, with exactly <int> e.g. 3 stops.
                { InputType.GetTripsByMaximumStops, "" },

                
                // TODO: Figure out why you made these...
                // TODO: What defines a journey?
                { InputType.GetJourney, "" },

                { InputType.InvalidInput, "Invalid input, please try again. \n" },
                { InputType.Help, m_InitialUserPrompt }
                // TODO: What about exit?
            };
        }

        private static void BuildInitialUserPrompt()
        {
            m_InitialUserPrompt = "Please enter a number between 1 - 6 from the options below. \n";

            foreach (var key in m_InputToOutput.Keys)
            {
                var inputType = m_InputToOutput[key];
                m_InitialUserPrompt += key + ". " + m_PromptTextByInputType[inputType];
            }
        }

        private static string Execute(string input, InputType inputType, ITripManager tripManager)
        {
            switch (inputType)
            {
                case InputType.GetCountOfTripsForStationsByStops:
                    return HandleGetCountOfTripsForStationsByStops(input, tripManager);
                default:
                    return m_PromptTextByInputType[InputType.InvalidInput];
            }

            /*tripManager.GetCountOfTripsForStationsByStops();
            tripManager.GetShorestRouteDistance();
            tripManager.GetJourney();
            tripManager.GetPermutations();
            tripManager.GetTripsByDistance();
            tripManager.GetTripsByMaximumStops();*/
        }

        private static InputType ProcessInput(string input, InputType expectedInputType)
        {
            return InputType.InvalidInput;
        }

        private static string HandleGetCountOfTripsForStationsByStops(string inputString, ITripManager tripManager)
        {
            var sourceStation = "";
            var destinationStation = "";
            int maximumStops = 0;

            return tripManager.GetCountOfTripsForStationsByStops(sourceStation, destinationStation, maximumStops).ToString();
        }
    }
}
