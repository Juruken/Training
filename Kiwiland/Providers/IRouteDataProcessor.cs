using System.Collections.Generic;

namespace Kiwiland.Providers
{
    public interface IRouteDataProcessor
    {
        List<string> Process(List<string> rawRouteData);
    }
}
