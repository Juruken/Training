using System.Collections.Generic;

namespace Kiwiland.Providers
{
    public interface IRouteDataProvider
    {
        List<string> GetData();
    }
}
