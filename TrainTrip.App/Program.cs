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
        // TODO: Move this into a constants folder
        private enum InputType
        {
            InvalidInput,
            Help,
            Exit,
            GetJourneyDistance,
            GetRoutesByMaximumStops,
            GetRoutesByExactStops,
            GetShortestRouteByDistance,
            GetPermutationsByDistance
        }

        private static string UNKNOWN_ROUTE_MESSAGE = "NO SUCH ROUTE";
        private static string m_InitialUserPrompt;
        private static Dictionary<string, InputType> m_InputToOutputMapping;
        private static Dictionary<InputType, string> m_PromptTextByInputType;

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

            // m_InputToOutputMapping = tripFactory.GetInputToOutputMapping();
            BuildOptions();
            // m_PromptTextByInputType = tripFactory.GetPromptTextByInputType();
            BuildPromptText();
            BuildInitialUserPrompt();

            m_PromptTextByInputType.Add(InputType.Help, m_InitialUserPrompt);

            Output(m_InitialUserPrompt);

            // Loop until the user closes the program.
            while (true)
            {
                // Fetch User Input
                // Get option 1,2,3 etc.
                var input = Console.ReadLine();

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

                Output(m_PromptTextByInputType[inputType]);

                input = Console.ReadLine();

                if (input != null)
                    input = input.ToUpper();

                // Get Data for User Input
                // m_InputValidator = tripFactory.GetInputValidator();
                while (!IsExpectedInputValid(input, inputType))
                {
                    Output(m_PromptTextByInputType[InputType.InvalidInput]);
                    Output(m_PromptTextByInputType[inputType]);
                    input = Console.ReadLine();
                }
                
                string outputMessage;
                try
                {
                    // If something is wrong with the input, we expect TrainTrip library to throw an exception.
                    switch (inputType)
                    {
                        case InputType.GetJourneyDistance:
                        case InputType.GetShortestRouteByDistance:
                            outputMessage = ExecuteGetJourneyDistanceOrShortestRoute(input, inputType, tripManager);
                            break;
                        case InputType.GetRoutesByExactStops:
                        case InputType.GetRoutesByMaximumStops:
                        case InputType.GetPermutationsByDistance:
                            outputMessage = ExecuteGetRoutesByExactStopsOrMaximumStopsOrGetPermutations(input, inputType, tripManager);
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

                Output("\n");
                Output("Please enter another number between 1 and 5.");
            }
        }
        
        private static void Output(string output)
        {
            Console.WriteLine(output);
        }

        // TODO: Move this into a factory!
        private static void BuildOptions()
        {
            m_InputToOutputMapping = new Dictionary<string, InputType>
            {
                { "1", InputType.GetJourneyDistance },
                { "2", InputType.GetRoutesByMaximumStops },
                { "3", InputType.GetRoutesByExactStops },
                { "4", InputType.GetShortestRouteByDistance },
                { "5", InputType.GetPermutationsByDistance },
                { "help", InputType.Help },
                { "exit", InputType.Exit }
            };
        }
        
        // TODO: Move this to the factory!
        private static void BuildPromptText()
        {
            m_PromptTextByInputType = new Dictionary<InputType, string>
            {
                { InputType.GetJourneyDistance, "The distance of a given route, expected format <StationName>-<StationName> (e.g. A-B or A-B-C)." },
                { InputType.GetRoutesByMaximumStops, "The number of trips starting at <StationName> ending at <StationName> with a maximum of <int> stops, expected format: <StationName><StationName><MaximumStops> e.g. CC3." },
                { InputType.GetRoutesByExactStops, "The number of trips starting at <StationName> ending at <StationName>, with exactly <int> stops (e.g. AC4)." },
                { InputType.GetShortestRouteByDistance, "The length of the shortest route (by distance) from <StationName> to <StationName>, expected format  <StationName>-<StationName> (e.g. A-C)." },
                { InputType.GetPermutationsByDistance, "The number of different routes from <StationName> to <StationName> with maximum of <int> expected format <StationName><StationName><MaxDistance>: (e.g. CC30)."},
                { InputType.InvalidInput, "Invalid input, please try again." },
            };
        }

        private static void BuildInitialUserPrompt()
        {
            m_InitialUserPrompt = "Please enter a number between 1 and 5 from the options below. You may also type 'help' to see this message again or 'exit' to close the program.\n";

            foreach (var key in m_InputToOutputMapping.Keys)
            {
                var inputType = m_InputToOutputMapping[key];

                // Help and exit should NOT be part of the initial user prompt.
                if (inputType == InputType.Help || inputType == InputType.Exit)
                {
                    continue; ;
                }

                m_InitialUserPrompt += key + ". " + m_PromptTextByInputType[inputType] + "\n";
            }
        }

        private static string ExecuteGetJourneyDistanceOrShortestRoute(string input, InputType inputType, ITripManager tripManager)
        {
            string[] stations = input.Split('-');

            if (inputType == InputType.GetJourneyDistance)
            {
                var journey = tripManager.GetJourney(stations);
                return journey != null ? journey.Distance.ToString() : UNKNOWN_ROUTE_MESSAGE;
            }

            if (inputType == InputType.GetShortestRouteByDistance)
            {
                var trip = tripManager.GetDirectRouteByLowestDistance(stations[0], stations[1]);
                return trip != null ? trip.TotalDistance.ToString() : UNKNOWN_ROUTE_MESSAGE;
            }

            throw new Exception("Unexpected Input Type");
        }

        private static string ExecuteGetRoutesByExactStopsOrMaximumStopsOrGetPermutations(string input, InputType inputType, ITripManager tripManager)
        {
            string sourceStation = input[0].ToString();
            string destinationStation = input[1].ToString();
            int maximumDistance = int.Parse(input.Substring(2));

            if (inputType == InputType.GetRoutesByMaximumStops)
            {
                return tripManager.GetRoutesByMaximumStops(sourceStation, destinationStation, maximumDistance).ToString();
            }

            if (inputType == InputType.GetRoutesByExactStops)
            {
                return tripManager.GetExactTripPermutationsCountByStops(sourceStation, destinationStation, maximumDistance).ToString();
            }

            if (inputType == InputType.GetPermutationsByDistance)
            {
                return tripManager.GetTripPermutationsCountByDistance(sourceStation, destinationStation, maximumDistance).ToString();
            }

            throw new Exception("Unexpected Input Type");
        }

        // TODO: Move this into a UserInputValidator
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

        // TODO: Move this into a UserInputValidator
        private static bool IsExpectedInputValid(string input, InputType expectedInputType)
        {
            // Assume input is invalid by default
            bool isValid = false;

            switch (expectedInputType)
            {
                // "The distance of a given route, expected format <StationName>-<StationName> e.g. A-B or A-B-C"
                case InputType.GetJourneyDistance:
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
                // "The number of trips starting at <StationName> ending at <StationName> with a maximum of <int> stops expected format: <StationName><StationName><int> e.g. CC3."
                case InputType.GetRoutesByMaximumStops:
                // "The number of trips starting at <StationName> ending at <StationName>, with exactly <int> stops (e.g. AC4)."
                case InputType.GetRoutesByExactStops:
                // "The number of different routes from <StationName> to <StationName> with maximum of <int> expected format <StationName><StationName><MaxDistance>: e.g. CC30"
                case InputType.GetPermutationsByDistance:
                    if (input.Length < 3 || !char.IsLetter(input[0]) || !char.IsLetter(input[1]))
                        break;

                    int num;
                    var possibleNumber = input.Substring(2);

                    isValid = int.TryParse(possibleNumber, out num);
                    break;
                default:
                    throw new Exception("Unknown input type");
            }

            return isValid;
        }
    }
}
