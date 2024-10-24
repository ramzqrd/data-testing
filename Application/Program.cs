using Models;

namespace Application
{
    //*********************************
    // This solution requires .NET 8 
    //*********************************

    public class Program
    {
        static void Main(string[] args)
        {
            CreateErrors(); // Run this whenever you want to generate a new set of data with errors
            CreateProviders(); // Run to generate data from mock data providers
        }

        private static void CreateErrors()
        {
            Data.CreateErrorData(Data.Objective, Data.ObjectiveError); // Create data with objective errors
            Data.CreateErrorData(Data.Subjective, Data.SubjectiveError); // Create data with subjective errors
            Data.CreateErrorData(Data.Mixed, Data.MixedError); // Create data with with a mix of both subjective and objective errors
        }

        private static void CreateProviders() // Create multiple providers with different error rates
        {
            Data.ERRORPERCENTAGE = 0.06;
            Data.ERRORRATE = 0.10;
            Data.CreateErrorData(Data.Provider1, Data.SubjectiveError);

            Data.ERRORPERCENTAGE = 0.12;
            Data.ERRORRATE = 0.20;
            Data.CreateErrorData(Data.Provider2, Data.SubjectiveError);

            Data.ERRORPERCENTAGE = 0.20;
            Data.ERRORRATE = 0.30;
            Data.CreateErrorData(Data.Provider3, Data.SubjectiveError);
        }
    }
}
