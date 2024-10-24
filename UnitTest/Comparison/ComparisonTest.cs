using static UnitTest.Functions;

namespace Comparison
{
    [TestClass]
    public class ComparisonTest
    {
        private static double Best = 0;
        private static Dictionary<string, double> TotalError = new();
        private static double Worst = 0;
        
        static ComparisonTest()
        {
            // Calculates the Square Error for each provider and gets the best/worst
            double err12 = CumulativeSquareError(Models.Data.Provider1, Models.Data.Provider2);
            double err23 = CumulativeSquareError(Models.Data.Provider2, Models.Data.Provider3);
            double err13 = CumulativeSquareError(Models.Data.Provider1, Models.Data.Provider3);
            TotalError[nameof(Models.Data.Provider1)] = err12 + err13;
            TotalError[nameof(Models.Data.Provider2)] = err12 + err23;
            TotalError[nameof(Models.Data.Provider3)] = err13 + err23;
            Best = TotalError.Values.Min();
            Worst = TotalError.Values.Max();
        }

        // Run this test to evaluate if provider1 is the best among the rest
        [TestMethod]
        public void Provider1_Is_Best()
        {
            Assert.IsTrue(TotalError[nameof(Models.Data.Provider1)] == Best);
        }

        // Run this test to evaluate if provider1 is among the top 2 out of the 3
        [TestMethod]
        public void Provider1_Is_Not_Worst()
        {
            Assert.IsTrue(TotalError[nameof(Models.Data.Provider1)] != Worst);
        }

        // Run this test to evaluate if provider2 is the best among the rest
        [TestMethod]
        public void Provider2_Is_Best()
        {
            Assert.IsTrue(TotalError[nameof(Models.Data.Provider2)] == Best);
        }

        // Run this test to evaluate if provider2 is among the top 2 out of the 3
        [TestMethod]
        public void Provider2_Is_Not_Worst()
        {
            Assert.IsTrue(TotalError[nameof(Models.Data.Provider2)] != Worst);
        }

        // Run this test to evaluate if provider3 is the best among the rest
        [TestMethod]
        public void Provider3_Is_Best()
        {
            Assert.IsTrue(TotalError[nameof(Models.Data.Provider3)] == Best);
        }

        // Run this test to evaluate if provider3 is among the top 2 out of the 3
        [TestMethod]
        public void Provider3_Is_Not_Worst()
        {
            Assert.IsTrue(TotalError[nameof(Models.Data.Provider3)] != Worst);
        }
    }
}