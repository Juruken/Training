﻿using System.Collections.Generic;

namespace Kiwiland.Processors
{
    public interface IStationDataProvider
    {
        IEnumerable<string> GetData();
    }
}
