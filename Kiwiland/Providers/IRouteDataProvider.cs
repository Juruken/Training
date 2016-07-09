using System.Collections.Generic;

namespace Kiwiland.Providers
{
    public interface IRouteDataProvider
    {
        /// <summary>
        /// Returns a list of Routes in the format: "AB1"
        /// Where A is the source station initial, B is the destination station initial, and 1 is the distance.
        /// </summary>
        /// <returns></returns>
        List<string> GetData();
    }
}
