using Models;

namespace Application
{
    public class Program
    {
        static void Main(string[] args)
        {
            CreateErrors(); // Run this whenever you want to generate a new set of data with errors
        }

        private static void CreateErrors()
        {
            Data.CreateErrorData(Data.Objective, Data.ObjectiveError);
            Data.CreateErrorData(Data.Subjective, Data.SubjectiveError);
            Data.CreateErrorData(Data.Mixed, Data.MixedError);
        }
    }
}
