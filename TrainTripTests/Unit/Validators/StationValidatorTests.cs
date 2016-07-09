using TrainTrip.Validators;
using NUnit.Framework;

namespace TrainTripTests.Validators
{
    [TestFixture]
    public class StationValidatorTests
    {
        private IStationDataValidator m_StationDataValidator;

        [SetUp]
        public void SetUp()
        {
            m_StationDataValidator = new StationDataValidator();
        }

        [TestCase("AB1", true)]
        [TestCase("AB3000", true)]
        [TestCase("ab1", true)]
        [TestCase("ab0", false, Reason = "Station string cannot have a distance of 0")]
        [TestCase("ab-1", false, Reason = "Station string cannot have any non-alphanumeric characters")]
        [TestCase("1ab", false, Reason = "Station string must start with a non-numeric character")]
        [TestCase("AB", false, Reason = "Station string is expected to be at least 3 characters")]
        [TestCase("ABB", false, Reason = "Station string must contain a distance")]
        [TestCase("ABB1", false, Reason = "Station string must be in the format <char><char><int>")]
        public void TestFormat(string stationString, bool expectedResult)
        {
            Assert.AreEqual(expectedResult, m_StationDataValidator.Validate(stationString));
        }
    }
}
