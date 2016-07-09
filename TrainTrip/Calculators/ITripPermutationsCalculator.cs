﻿using System.Collections.Generic;
using TrainTrip.Data;

namespace TrainTrip.Calculators
{
    public interface ITripPermutationsCalculator
    {
        List<Trip> GetPermutations(string sourceStation, string destinationStation, int maximumDistance);
    }
}