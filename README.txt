A stand alone version of the application can be found in Release folder.

If you wish to replace the source train stations file you may, but you will need to update the 'TrainTrip.App.exe.config' file
to point to your new source file.

If you have a delimiter other than ',' in your source file, you will also need to update 'TrainTrip.App.exe.config'.

The solution was developed in Visual Studio 2015 Community Edition.
Using Resharper 2016 1.2.

The Unit and Component tests are dependent on NUnit 3.4.1 and Moq 4.5.10, Moq is dependent on Castle.Core 3.3.3.

They are downloaded using NuGet from source: https://api.nuget.org/v3/index.json.


To confirm the the coding test works you can run: "CodingTestComponentTests" to mimic the user inputing the test inputs.


// GetJourneyDistance
1. The distance of the route A-B-C.
2. The distance of the route A-D.
3. The distance of the route A-D-C.
4. The distance of the route A-E-B-C-D.
5. The distance of the route A-E-D.

// GetRoutesByMaximumStops
// Calculate all routes to C to C with a max of 3 stops. i.e. return a count where the length of the TripName is <= 4 (e.g. CDC, CEBC).
6. The number of trips starting at C and ending at C with a maximum of 3 stops.  
	In the sample data below, there are two such trips: C-D-C (2 stops). and C-E-B-C (3 stops).

// GetRoutesByExactStops
// Calculate all permutations for A - C. Return a Count where the length of the TripName == 5 (e.g. ABCDC, ADCDC, ADEBC)
7. The number of trips starting at A and ending at C with exactly 4 stops.  
	In the sample data below, there are three such trips: 
		A to C (via B,C,D); A to C (via D,C,D); and A to C (via D,E,B).

// GetShortestRouteByDistance
8. The length of the shortest route (in terms of distance to travel) from A to C.
9. The length of the shortest route (in terms of distance to travel) from B to B.

// GetPermutations
10. The number of different routes from C to C with a distance of less than 30.  
	In the sample data, the trips are: CDC, CEBC, CEBCDC, CDCEBC, CDEBC, CEBCEBC, CEBCEBCEBC.
