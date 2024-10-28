using static UnitTest.Functions;

namespace Data
{
    [TestClass]
    public class CleanTest
    {
        private readonly string Folder = Models.Data.Clean;

        // Run this test to verify all dates returned are in the past
        [TestMethod]
        public void All_Dates_Are_Past()
        {
            Assert.IsTrue(PastDates(Folder));
        }

        // Run this test to verify all data files are readable
        [TestMethod]
        public void All_Files_Readable()
        {
            Assert.IsTrue(AllFilesReadable(Folder));
        }

        // Run this test to verify if all dates returned are valid trading days
        [TestMethod]
        public void All_Dates_Are_Valid_Trading_Days()
        {
            Assert.IsTrue(ValidTradingDays(Folder));
        }

        // Run this test to verify there are no non numeric values in the data
        [TestMethod]
        public void No_NAN_Values()
        {
            Assert.IsTrue(NoNAN(Folder));
        }

        // Run this test to verify all values returned for volume are positive
        [TestMethod]
        public void All_Volumes_Are_Positive()
        {
            Assert.IsTrue(PositiveVolumes(Folder));
        }

        // Run this test to verify all values returned are positive
        [TestMethod]
        public void OHLC_Must_All_Be_Positive()
        {
            Assert.IsTrue(OHLCPositive(Folder));
        }

        // Run this test to verify if all values returned are consistent to basic market rules
        [TestMethod]
        public void OHLC_Values_Are_Inconsistent()
        {
            Assert.IsTrue(OHLCConsistent(Folder));
        }
    }
}