using System.Collections.Generic;

namespace TrainTrip
{
    public interface IFileDataProvider
    {
        List<string> GetFileData();
    }
}
