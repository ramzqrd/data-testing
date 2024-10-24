using System.Globalization;
using static System.Math;
using Values = (System.DateTime Date, double Open, double High, double Low, double Close, double Volume);

namespace UnitTest
{
    public static class Functions
    {
        // This method checks if all files in the specified folder are readable and can be processed without errors.
        public static bool AllFilesReadable(string folder)
        {
            // Get all file paths from the specified folder.
            string[] files = Directory.GetFiles(folder);

            // Loop through each file in the folder.
            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    string source = files[i]; // Get the file path.

                    // Read all lines from the file, skipping the header.
                    string[] lines = File.ReadAllLines(source).Skip(1).ToArray();

                    // Convert each line into a Values object.
                    List<Values> valueList = lines.Select(Values).ToList();
                }
                catch
                {
                    // If an exception occurs, return false indicating that not all files are readable.
                    return false;
                }
            }
            // If all files are readable, return true.
            return true;
        }


        internal static double CumulativeSquareError(string folder1, string folder2)
        {
            double cse = 0; // Initialize the cumulative squared error (cse) to 0.

            // Get the list of file names (without paths) from both folders.
            List<string> files1 = Directory.GetFiles(folder1).Select(f => Path.GetFileName(f)).ToList();
            List<string> files2 = Directory.GetFiles(folder2).Select(f => Path.GetFileName(f)).ToList();

            // Find the common files between the two folders.
            List<string> files = files1.Intersect(files2).ToList();

            // Loop over the common files and calculate the cumulative squared error.
            for (int i = 0; i < files.Count; i++)
            {
                // Construct the full path for each matching file in both folders.
                string file1 = Path.Combine(folder1, files1[i]);
                List<Values> values1 = GetValues(file1); // Get the historical data from the first folder's file.

                string file2 = Path.Combine(folder2, files2[i]);
                List<Values> values2 = GetValues(file2); // Get the historical data from the second folder's file.

                // Calculate the square distance between the two series and accumulate it into cse.
                cse += HistoricalSeriesSquareDistance(values1, values2);
            }
            return cse; // Return the total cumulative squared error.
        }


        /* Computes the "square distance" between two historical price series by finding common dates, 
           then summing the squared logarithmic differences of the Open, High, Low, and Close prices 
           for those dates. Returns the total squared error (hse). */
        private static double HistoricalSeriesSquareDistance(List<Values> values1, List<Values> values2)
        {
            double hse = 0; // Initialize the total squared error (hse) to 0.

            // Find common dates between the two price series.
            List<DateTime> dates = values1.Select(v => v.Date).Intersect(values2.Select(v => v.Date)).ToList();

            // Remove entries that don't have matching dates in both series.
            values1.RemoveAll(v => !dates.Contains(v.Date));
            values2.RemoveAll(v => !dates.Contains(v.Date));

            // Loop over the common dates to calculate the squared distance.
            for (int i = 0; i < dates.Count; i++)
            {
                Values v1 = values1[i]; // Get the value from the first series.
                Values v2 = values2[i]; // Get the corresponding value from the second series.

                // Calculate and accumulate the squared logarithmic differences for Open, High, Low, and Close prices.
                hse += Pow(Log(v1.Open / v2.Open), 2);
                hse += Pow(Log(v1.High / v2.High), 2);
                hse += Pow(Log(v1.Low / v2.Low), 2);
                hse += Pow(Log(v1.Close / v2.Close), 2);
            }
            return hse; // Return the total squared error.
        }

        // This method creates a list of the prices by reading a file
        public static List<Values> GetValues(string file)
        {
            // Read all lines from the file, skipping the header.
            string[] lines = File.ReadAllLines(file).Skip(1).ToArray();

            // Convert each line into a Values object and return the list.
            List<Values> valueList = lines.Select(Values).ToList();
            return valueList;
        }

        // This method ensures there are no non numberic values in the data
        public static bool NoNAN(string folder)
        {
            // Get all file paths from the specified folder.
            string[] files = Directory.GetFiles(folder);

            // Loop through each file in the folder.
            for (int i = 0; i < files.Length; i++)
            {
                string source = files[i]; // Get the file path.

                // Read all lines from the file, skipping the header.
                string[] lines = File.ReadAllLines(source).Skip(1).ToArray();

                // Convert each line into a Values object.
                List<Values> valueList = lines.Select(Values).ToList();

                // Check if any of the Open, High, Low, or Close values contain NaN. If so, return false.
                if (valueList.SelectMany(v => new double[] { v.Open, v.High, v.Low, v.Close }).Any(double.IsNaN))
                    return false;
            }
            // If no NaN values are found in any file, return true.
            return true;
        }

        // This method performs sanity checks to ensure the data upholds to basic market rules
        private static bool IsOHLCConsistent(Values values)
        {
            if (values.Open > values.High || values.Low > values.High || values.Close > values.High) return false;
            if (values.Open < values.Low || values.High < values.Low || values.Close < values.Low) return false;
            return true;
        }

        public static bool OHLCConsistent(string folder)
        {
            // Get all file paths from the specified folder.
            string[] files = Directory.GetFiles(folder);

            // Loop through each file in the folder.
            for (int i = 0; i < files.Length; i++)
            {
                string source = files[i]; // Get the file path.

                // Read all lines from the file, skipping the header.
                string[] lines = File.ReadAllLines(source).Skip(1).ToArray();

                // Convert each line into a Values object.
                List<Values> valueList = lines.Select(Values).ToList();

                // Check if any OHLC (Open, High, Low, Close) values are inconsistent. If so, return false.
                if (valueList.Any(v => !IsOHLCConsistent(v)))
                    return false;
            }
            // If all OHLC values are consistent in every file, return true.
            return true;
        }

        public static bool OHLCPositive(string folder)
        {
            // Get all file paths from the specified folder.
            string[] files = Directory.GetFiles(folder);

            // Loop through each file in the folder.
            for (int i = 0; i < files.Length; i++)
            {
                string source = files[i]; // Get the file path.

                // Read all lines from the file, skipping the header.
                string[] lines = File.ReadAllLines(source).Skip(1).ToArray();

                // Convert each line into a Values object.
                List<Values> valueList = lines.Select(Values).ToList();

                // Check if any OHLC (Open, High, Low, Close) values are negative. If so, return false.
                if (valueList.SelectMany(v => new double[] { v.Open, v.High, v.Low, v.Close }).Any(v => v < 0))
                    return false;
            }
            // If all OHLC values are positive in every file, return true.
            return true;
        }

        // This method checks if all dates in the files within the specified folder are in the past.
        public static bool PastDates(string folder)
        {
            string[] files = Directory.GetFiles(folder);
            for (int i = 0; i < files.Length; i++)
            {
                string source = files[i];

                // Read all lines from the file, skipping the header.
                string[] lines = File.ReadAllLines(source).Skip(1).ToArray();

                // Convert each line into a Values object.
                List<Values> valueList = lines.Select(Values).ToList();

                // If any date is in the future, return false.
                if (valueList.Any(v => v.Date > DateTime.Now)) return false;
            }
            // If all dates are in the past, return true.
            return true;
        }

        // This method checks if all volume values in the files within the specified folder are positive.
        public static bool PositiveVolumes(string folder)
        {
            string[] files = Directory.GetFiles(folder);
            for (int i = 0; i < files.Length; i++)
            {
                string source = files[i];

                // Read all lines from the file, skipping the header.
                string[] lines = File.ReadAllLines(source).Skip(1).ToArray();

                // Convert each line into a Values object.
                List<Values> valueList = lines.Select(Values).ToList();

                // If any volume is negative, return false.
                if (valueList.Any(v => v.Volume < 0)) return false;
            }
            // If all volumes are positive, return true.
            return true;
        }

        // Checks if all trading days in the files within the specified folder are valid (i.e., not on weekends).
        public static bool ValidTradingDays(string folder)
        {
            string[] files = Directory.GetFiles(folder);
            for (int i = 0; i < files.Length; i++)
            {
                string source = files[i];

                // Read all lines from the file, skipping the header.
                string[] lines = File.ReadAllLines(source).Skip(1).ToArray();

                // Convert each line into a Values object.
                List<Values> valueList = lines.Select(Values).ToList();

                // If any date falls on a Saturday or Sunday, return false.
                if (valueList.Any(v => v.Date.DayOfWeek == DayOfWeek.Saturday || v.Date.DayOfWeek == DayOfWeek.Sunday)) return false;
            }
            // If all trading days are valid, return true.
            return true;
        }

        // Converts a CSV line into a Values object by parsing date and numeric values.
        public static Values Values(string line)
        {
            // Split the line by commas and trim whitespace from each value.
            string[] values = line.Split(',').Select(v => v.Trim()).ToArray();

            // Parse the date and numeric values using invariant culture.
            DateTime date = DateTime.Parse(values[0], CultureInfo.InvariantCulture);
            double open = double.Parse(values[1], CultureInfo.InvariantCulture);
            double high = double.Parse(values[2], CultureInfo.InvariantCulture);
            double low = double.Parse(values[3], CultureInfo.InvariantCulture);
            double close = double.Parse(values[4], CultureInfo.InvariantCulture);
            double volume = double.Parse(values[5], CultureInfo.InvariantCulture);

            // Return a Values object containing the parsed data.
            return (date, open, high, low, close, volume);
        }
    }
}
