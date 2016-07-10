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
                        return;
                }

                // Get Data for User Input
                if (!IsExpectedInputValid(input, inputType))
                {
                    Output(m_PromptTextByInputType[InputType.Help]);
                    continue;
                }

                string result;
                try
                {
                    // If something is wrong with the input, we expect TrainTrip library to throw an exception.
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

        // TODO: MOVE ALL OF THIS... to testable stuff soon.

        private static InputType ValidateInitialOptionInput(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
                return InputType.InvalidInput;

            inputString = inputString.ToLower();

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
            m_PromptTextByInputType = new Dictionary<InputType, string>
            {
                // 1. GetJourney
                // 2. GetJourney
                // 3. GetJourney
                // 4. GetJourney
                // 5. GetJourney
                // 6. GetRoutesByMaximumStops
                // 7. GetRoutesByExactStops
                // 8. GetShortestRouteByDistance
                // 9. GetShortestRouteByDistance
                // 10. GetPermutations

                { InputType.GetJourney, "The distance of a given route, expected format <StationName>-<StationName> e.g. A-B or A-B-C \n" },
                { InputType.GetRoutesByMaximumStops, "The number of trips starting at <StationName> ending at <StationName> with a maximum of <int> stops expected format: <StationName><StationName><int> e.g. CC3." },
                { InputType.GetRoutesByExactStops, "The number of trips starting at <StationName> e.g. A ending at <StationName> e.g C, with exactly <int> e.g. 3 stops." },
                { InputType.GetShortestRouteByDistance, "The length of the shortest route (by distance) from <StationName> to <StationName>, expected format  <StationName>-<StationName> e.g. A-C" },
                { InputType.GetPermutations, "The number of different routes from <StationName> to <StationName> with maximum of <int> expected format <StationName><StationName><MaxDistance>: e.g. CC30"},
                
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
            // 1. GetJourney
            // 2. GetJourney
            // 3. GetJourney
            // 4. GetJourney
            // 5. GetJourney
            // 6. GetRoutesByMaximumStops
            // 7. GetRoutesByExactStops
            // 8. GetShortestRouteByDistance
            // 9. GetShortestRouteByDistance
            // 10. GetPermutations

            /*{ InputType.GetJourney, "The distance of a given route, expected format <StationName>-<StationName> e.g. A-B or A-B-C \n" },
            { InputType.GetRoutesByMaximumStops, "The number of trips starting at <StationName> ending at <StationName> with a maximum of <int> stops expected format: <StationName><StationName><int> e.g. CC3." },
            { InputType.GetRoutesByExactStops, "The number of trips starting at <StationName> e.g. A ending at <StationName> e.g C, with exactly <int> e.g. 3 stops." },
            { InputType.GetShortestRouteByDistance, "The length of the shortest route (by distance) from <StationName> to <StationName>, expected format  <StationName>-<StationName> e.g. A-C" },
            { InputType.GetPermutations, "The number of different routes from <StationName> to <StationName> with maximum of <int> expected format <StationName><StationName><MaxDistance>: e.g. CC30"},*/

            // TODO: Deal with this... /sigh.
            switch (inputType)
            {
                case InputType.GetJourney:
                    break;
                case InputType.GetRoutesByMaximumStops:
                    break;
                case InputType.GetRoutesByExactStops:
                    break;
                case InputType.GetShortestRouteByDistance:
                    break;
                case InputType.GetPermutations:
                    break;
                default:
                    return m_PromptTextByInputType[InputType.InvalidInput];
            }
            
            // TODO: Delete this
            return null;
        }

        // TODO: Make this return true if the expected input type was valid
        private static bool IsExpectedInputValid(string input, InputType expectedInputType)
        {
            // Assume input is invalid by default
            bool isValid = false;

            switch (expectedInputType)
            {
                // "The distance of a given route, expected format <StationName>-<StationName> e.g. A-B or A-B-C \n"
                case InputType.GetJourney:
                    break;
                // "The number of trips starting at <StationName> ending at <StationName> with a maximum of <int> stops expected format: <StationName><StationName><int> e.g. CC3."
                case InputType.GetRoutesByMaximumStops:
                    break;
                // "The number of trips starting at <StationName> e.g. A ending at <StationName> e.g C, with exactly <int> e.g. 3 stops."
                case InputType.GetRoutesByExactStops:
                    break;
                // "The length of the shortest route (by distance) from <StationName> to <StationName>, expected format  <StationName>-<StationName> e.g. A-C"
                case InputType.GetShortestRouteByDistance:
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
