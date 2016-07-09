namespace Kiwiland.Validators
{
    // TODO: Split these into IDataValidator implementations
    // TODO: Then inject them as a list of IDataValidation rules into the IRouteProcess and IRouteProcessor
    public class RouteDataValidator : IRouteDataValidator
    {
        public bool Validate(string routeString)
        {
            return ValidLength(routeString)
                   && ValidStartingCharacter(routeString)
                   && ValidDistance(routeString)
                   && ValidNumberFormat(routeString);
        }

        /// <summary>
        /// Returns true if there is a non number character detected after the third index e.g. AB1A
        /// </summary>
        /// <param name="routeString"></param>
        /// <returns></returns>
        internal bool ValidNumberFormat(string routeString)
        {
            if (routeString.Length > 3)
            {
                var expectedDistance = routeString.Substring(2);
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
        /// <param name="routeString"></param>
        /// <returns></returns>
        internal bool ValidDistance(string routeString)
        {
            return char.IsNumber(routeString[2]) && int.Parse(routeString[2].ToString()) > 0;
        }

        internal bool ValidStartingCharacter(string routeString)
        {
            return char.IsLetter(routeString[0]);
        }

        internal bool ValidLength(string routeString)
        {
            return routeString.Length >= 3;
        }
    }
}
