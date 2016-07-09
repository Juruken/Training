namespace Kiwiland.Validators
{
    // TODO: Split these into IDataValidator implementations
    // TODO: Then inject them as a list of IDataValidation rules into the IStationProcess and IRouteProcessor
    public class StationDataValidator : IStationDataValidator
    {
        public bool Validate(string stationString)
        {
            return ValidLength(stationString)
                   && ValidStartingCharacter(stationString)
                   && ValidDistance(stationString)
                   && ValidNumberFormat(stationString);
        }

        /// <summary>
        /// Returns true if there is a non number character detected after the third index e.g. AB1A
        /// </summary>
        /// <param name="stationString"></param>
        /// <returns></returns>
        internal bool ValidNumberFormat(string stationString)
        {
            if (stationString.Length > 3)
            {
                var expectedDistance = stationString.Substring(2);
                foreach (var character in expectedDistance)
                {
                    if (!char.IsNumber(character))
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// A valid distance must be greater than 0. Will not accept AB01.
        /// </summary>
        /// <param name="stationString"></param>
        /// <returns></returns>
        internal bool ValidDistance(string stationString)
        {
            return char.IsNumber(stationString[2]) && int.Parse(stationString[2].ToString()) > 0;
        }

        internal bool ValidStartingCharacter(string stationString)
        {
            return char.IsLetter(stationString[0]);
        }
        
        internal bool ValidLength(string stationString)
        {
            return stationString.Length >= 3;
        }
    }
}
