using Models;

namespace Application
{
    /*********************************
     This solution requires .NET 8 
    *********************************/

    public class Program
    {
        // The main method is the starting point of the program and manages the flow of the application.
        static void Main(string[] args)
        {
            CreateErrors(); // Run this whenever you want to generate a new set of data with errors
            CreateProviders(); // Run to generate data from mock data providers
        }

        private static void CreateErrors()
        {
            Data.CreateErrorData(Data.Objective, Data.ObjectiveError); // Create data with objective errors
            Data.CreateErrorData(Data.Subjective, Data.SubjectiveError); // Create data with subjective errors
            Data.CreateErrorData(Data.Mixed, Data.MixedError); // Create data with a mix of both subjective and objective errors
        }

        /* This method generates multiple datasets with varying error rates and percentages, which will be utilized to rank the resulting datasets
         according to their accuracy using the least squares distance method. */
        private static void CreateProviders() { 
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
