using System;
using TrainTrip.Constants;

namespace TrainTrip.Validators
{
    public class InputValidator : IInputValidator
    {
        public bool Validate(string input, InputType inputType)
        {
            // Assume input is invalid by default
            bool isValid = false;

            switch (inputType)
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
                    throw new ArgumentException("Unknown input type");
            }

            return isValid;
        }
    }
}
