using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
            GetJourney,
            GetRoutesByMaximumStops,
            GetRoutesByExactStops,
            GetShortestRouteByDistance,
            GetPermutations
        }

        private static string UNKNOWN_ROUTE_MESSAGE = "NO SUCH ROUTE";
        private static string m_InitialUserPrompt;
        private static Dictionary<string, InputType> m_InputToOutputMapping;
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
                        return;
                }

                // Get Data for User Input
                // m_InputValidator = tripFactory.GetInputValidator();
                if (!IsExpectedInputValid(input, inputType))
                {
                    Output(m_PromptTextByInputType[InputType.Help]);
                    continue;
                }

                string outputMessage;
                try
                {
                    // If something is wrong with the input, we expect TrainTrip library to throw an exception.
                    switch (inputType)
                    {
                        case InputType.GetJourney:
                        case InputType.GetShortestRouteByDistance:
                            outputMessage = ExecuteGetJourneyDistanceOrShortestRoute(input, inputType, tripManager);
                            break;
                        case InputType.GetRoutesByMaximumStops:
                        case InputType.GetPermutations:
                            outputMessage = ExecuteGetRoutesByMaximumStopsOrGetPermutations(input, inputType, tripManager);
                            break;
                        default:
                            outputMessage = UNKNOWN_ROUTE_MESSAGE;
                        break;
                    }
                }
                catch (InvalidRouteException)
                {
                    outputMessage = UNKNOWN_ROUTE_MESSAGE;
                }
                catch (TrainTripException ex)
                {
                    outputMessage = ex.Message;
                }

                // Output to Console
                Output(outputMessage);
            } while (input != "Exit");
        }

        // TODO: MOVE ALL OF THIS... to testable stuff soon.

        private static InputType ValidateInitialOptionInput(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
                return InputType.InvalidInput;

            inputString = inputString.ToLower();

            if (m_InputToOutputMapping.ContainsKey(inputString))
                return m_InputToOutputMapping[inputString];

            // If we don't know what it is, assume it is invalid.
            return InputType.InvalidInput;
        }

        private static void Output(string output)
        {
            Console.WriteLine(output);
        }

        private static void BuildOptions()
        {
            // m_InputToOutputMapping = tripFactory.GetInputToOutputMapping();
            m_InputToOutputMapping = new Dictionary<string, InputType>
            {
                { "1", InputType.GetJourney },
                { "2", InputType.GetRoutesByMaximumStops },
                { "3", InputType.GetRoutesByExactStops },
                { "4", InputType.GetShortestRouteByDistance },
                { "5", InputType.GetPermutations },
                // TODO: Print available Routes
                // TODO: Print available Stations
                { "help", InputType.Help },
                { "exit", InputType.Exit }
            };
        }
        
        // TODO: Move this to the factory!
        private static void BuildPromptText()
        {
            // m_PromptTextByInputType = tripFactory.GetPromptTextByInputType();
            m_PromptTextByInputType = new Dictionary<InputType, string>
            {
                { InputType.GetJourney, "The distance of a given route, expected format <StationName>-<StationName> (e.g. A-B or A-B-C)." },
                { InputType.GetRoutesByMaximumStops, "The number of trips starting at <StationName> ending at <StationName> with a maximum of <int> stops expected format: <StationName><StationName><MaximumStops> e.g. CC3." },
                { InputType.GetRoutesByExactStops, "The number of trips starting at <StationName> ending at <StationName>, with exactly <int> stops (e.g. AC3)." },
                { InputType.GetShortestRouteByDistance, "The length of the shortest route (by distance) from <StationName> to <StationName>, expected format  <StationName>-<StationName> (e.g. A-C)." },
                { InputType.GetPermutations, "The number of different routes from <StationName> to <StationName> with maximum of <int> expected format <StationName><StationName><MaxDistance>: (e.g. CC30)."},
                { InputType.InvalidInput, "Invalid input, please try again." },
                { InputType.Help, m_InitialUserPrompt }
            };
        }

        private static void BuildInitialUserPrompt()
        {
            m_InitialUserPrompt = "Please enter a number between 1 - 6 from the options below. \n";

            foreach (var key in m_InputToOutputMapping.Keys)
            {
                var inputType = m_InputToOutputMapping[key];
                m_InitialUserPrompt += key + ". " + m_PromptTextByInputType[inputType];
            }
        }

        private static string ExecuteGetJourneyDistanceOrShortestRoute(string input, InputType inputType, ITripManager tripManager)
        {
            string[] stations = input.Split('-');

            if (inputType == InputType.GetJourney)
            {
                // TODO: Refactor out maxDistance.
                // TODO: Wrap this
                var journey = tripManager.GetJourney(stations, 1000, true);
                return journey != null ? journey.Distance.ToString() : UNKNOWN_ROUTE_MESSAGE;
            }

            if (inputType == InputType.GetShortestRouteByDistance)
            {
                // TODO: Refactor out maxDistance.
                // TODO: Wrap this
                var trip = tripManager.GetShortestRouteByDistance(stations[0], stations[1], 1000, true);
                return trip != null ? trip.TotalDistance.ToString() : UNKNOWN_ROUTE_MESSAGE;
            }

            throw new Exception("Unexpected Input Type");
        }

        private static string ExecuteGetRoutesByMaximumStopsOrGetPermutations(string input, InputType inputType, ITripManager tripManager)
        {
            string sourceStation = input.Substring(0, 1);
            string destinationStation = input.Substring(1, 1);
            int maximumDistance = int.Parse(input.Substring(2, 1));

            if (inputType == InputType.GetRoutesByMaximumStops)
            {
                var permutations = tripManager.GetRoutesByMaximumStops(sourceStation, destinationStation, maximumDistance);
                return permutations != null ? permutations.Count.ToString() : UNKNOWN_ROUTE_MESSAGE;
            }
            else if (inputType == InputType.GetPermutations)
            {
                var permutations = tripManager.GetPermutations(sourceStation, destinationStation, maximumDistance);
                return permutations != null ? permutations.Count.ToString() : UNKNOWN_ROUTE_MESSAGE;
            }

            throw new Exception("Unexpected Input Type");
        }

        private static bool IsExpectedInputValid(string input, InputType expectedInputType)
        {
            // Assume input is invalid by default
            bool isValid = false;

            switch (expectedInputType)
            {
                // 1. - 5. => GetJourney
                // "The distance of a given route, expected format <StationName>-<StationName> e.g. A-B or A-B-C"
                case InputType.GetJourney:
                // 8. && 9. => GetShortestRouteByDistance
                // "The length of the shortest route (by distance) from <StationName> to <StationName>, expected format  <StationName>-<StationName> e.g. A-C"
                case InputType.GetShortestRouteByDistance:
                    if (!input.Contains("-"))
                        break;

                    var stationNames = input.Split('-');

                    if (stationNames.Length < 2)
                        break;

                    foreach (var stationName in stationNames)
                    {
                        if (!char.IsLetter(stationName[0]))
                        {
                            break;
                        }
                    }

                    isValid = true;
                    break;
                // 6. GetRoutesByMaximumStops
                // "The number of trips starting at <StationName> ending at <StationName> with a maximum of <int> stops expected format: <StationName><StationName><int> e.g. CC3."
                case InputType.GetRoutesByMaximumStops:
                // 10. GetPermutations
                // "The number of different routes from <StationName> to <StationName> with maximum of <int> expected format <StationName><StationName><MaxDistance>: e.g. CC30"
                case InputType.GetPermutations:
                    if (input.Length < 3 || !char.IsLetter(input[0]) || !char.IsLetter(input[1]))
                        break;

                    int num;
                    var possibleNumber = input.Substring(2);

                    isValid = int.TryParse(possibleNumber, out num);
                    break;
            }

            return isValid;
        }
    }
}
