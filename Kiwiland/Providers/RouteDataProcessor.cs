using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Kiwiland.Providers;
using Kiwiland.Validators;

namespace Kiwiland.Processors
{
    /// <summary>
    /// Responsible for processing raw route data. If the data is in an unexpected format it will throw an exception.
    /// </summary>
    public class RouteDataProcessor : IRouteDataProcessor
    {
        private readonly char m_Delimiter;
        private readonly IRouteDataValidator m_RouteDataValidator;

        public RouteDataProcessor(IRouteDataValidator validator, char delimiter)
        {
            m_Delimiter = delimiter;
            m_RouteDataValidator = validator;
        }

        /// <summary>
        /// Expects a raw string of file lines, seperated by the injected delimeter.
        /// </summary>
        /// <returns>Processed Routes in their String format</returns>
        public List<string> Process(List<string> rawRouteData)
        {
            var routes = new List<string>();

            foreach (var line in rawRouteData)
            {
                var routeLine = line;
                if (m_Delimiter != ' ')
                {
                    routeLine = routeLine.Replace(" ", "");
                }

                var routeStrings = routeLine.Split(m_Delimiter);

                foreach (var route in routeStrings)
                {
                    if (!m_RouteDataValidator.Validate(route))
                    {
                        throw new ArgumentException("Route Data contains invalid format");
                    }

                    routes.Add(route);
                }
            }

            return routes.ToList();
        }
    }
}
