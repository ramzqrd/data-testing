using Models;
using static UnitTest.Functions;

namespace Data
{
    [TestClass]
    public class SubjectiveTest
    {
        private readonly string Folder = Models.Data.Subjective;

        [TestMethod]
        public void All_Dates_Are_Past()
        {
            Assert.IsTrue(PastDates(Folder));
        }

        [TestMethod]
        public void All_Files_Readable()
        {
            Assert.IsTrue(AllFilesReadable(Folder));
        }

        [TestMethod]
        public void All_Dates_Are_Valid_Trading_Days()
        {
            Assert.IsTrue(ValidTradingDays(Folder));
        }

        [TestMethod]
        public void No_NAN_Values()
        {
            Assert.IsTrue(NoNAN(Folder));
        }

        [TestMethod]
        public void All_Volumes_Are_Positive()
        {
            Assert.IsTrue(PositiveVolumes(Folder));
        }

        [TestMethod]
        public void OHLC_Must_All_Be_Positive()
        {
            Assert.IsTrue(OHLCPositive(Folder));
        }

        [TestMethod]
        public void OHLC_Values_Are_Inconsistent()
        {
            Assert.IsTrue(OHLCConsistent(Folder));
        }
    }
}