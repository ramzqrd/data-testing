using Models;
using static UnitTest.Functions;

namespace Comparison
{
    [TestClass]
    public class ProviderComparisonTest
    {
        private static double Best = 0;
        private static Dictionary<string, double> TotalError = new();
        private static double Worst = 0;

        static ProviderComparisonTest()
        {
            double err12 = CumulativeSquareError(Models.Data.Provider1, Models.Data.Provider2);
            double err23 = CumulativeSquareError(Models.Data.Provider2, Models.Data.Provider3);
            double err13 = CumulativeSquareError(Models.Data.Provider1, Models.Data.Provider3);
            TotalError[nameof(Models.Data.Provider1)] = err12 + err13;
            TotalError[nameof(Models.Data.Provider2)] = err12 + err23;
            TotalError[nameof(Models.Data.Provider3)] = err13 + err23;
            Best = TotalError.Values.Min();
            Worst = TotalError.Values.Max();
        }

        [TestMethod]
        public void Provider1_Is_Best()
        {
            Assert.IsTrue(TotalError[nameof(Models.Data.Provider1)] == Best);
        }

        [TestMethod]
        public void Provider1_Is_Not_Worst()
        {
            Assert.IsTrue(TotalError[nameof(Models.Data.Provider1)] != Worst);
        }

        [TestMethod]
        public void Provider2_Is_Best()
        {
            Assert.IsTrue(TotalError[nameof(Models.Data.Provider2)] == Best);
        }

        [TestMethod]
        public void Provider2_Is_Not_Worst()
        {
            Assert.IsTrue(TotalError[nameof(Models.Data.Provider2)] != Worst);
        }


        [TestMethod]
        public void Provider3_Is_Best()
        {
            Assert.IsTrue(TotalError[nameof(Models.Data.Provider3)] == Best);
        }

        [TestMethod]
        public void Provider3_Is_Not_Worst()
        {
            Assert.IsTrue(TotalError[nameof(Models.Data.Provider3)] != Worst);
        }
    }
}