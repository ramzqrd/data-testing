using System.Globalization;
using static System.Math;
using System.IO.Compression;

namespace Models
{
    // This class includes methods for reading data folders and introducing objective, subjective, and mixed errors into the original datasets based on a specified error rate and percentage.
    public static class Data
    {
        // Set default values for the artificial error rate (indicating the acceptable error level in the dataset) and the error percentage (applied to modify the original dataset values).
        public static double ERRORPERCENTAGE = 0.05;
        public static double ERRORRATE = 0.03;

        // The root data folder used is created on the desktop but can be changed
        public static readonly string Root = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), nameof(Data));

        // All other directories are located inside the root
        public static readonly string Clean = Path.Combine(Root, nameof(Clean));
        public static readonly string Mixed = Path.Combine(Root, nameof(Mixed));
        public static readonly string Objective = Path.Combine(Root, nameof(Objective));
        public static readonly string Subjective = Path.Combine(Root, nameof(Subjective));

        // These are the mock data providers
        public static readonly string Provider1 = Path.Combine(Root, nameof(Provider1));
        public static readonly string Provider2 = Path.Combine(Root, nameof(Provider2));
        public static readonly string Provider3 = Path.Combine(Root, nameof(Provider3));

        static Data()
        {
            // Ensure that the full directory structure is in place before any use of data
            if (!Directory.Exists(Root)) Directory.CreateDirectory(Root);
            if (!Directory.Exists(Clean)) Directory.CreateDirectory(Clean);
            if (!Directory.Exists(Mixed)) Directory.CreateDirectory(Mixed);
            if (!Directory.Exists(Objective)) Directory.CreateDirectory(Objective);
            if (!Directory.Exists(Subjective)) Directory.CreateDirectory(Subjective);
            if (!Directory.Exists(Provider1)) Directory.CreateDirectory(Provider1);
            if (!Directory.Exists(Provider2)) Directory.CreateDirectory(Provider2);
            if (!Directory.Exists(Provider3)) Directory.CreateDirectory(Provider3);
            if (Directory.GetFiles(Clean).Length == 0)
            {
                string zip = Path.Combine(nameof(Data), nameof(Clean) + ".zip");
                ZipFile.ExtractToDirectory(zip, Clean, true);
            }
        }

        // Return true if a random number is below the error rate.
        public static bool AddError() => Random.Shared.NextDouble() < ERRORRATE;

        // Delete existing files in order to generate new ones
        public static void CleanFolder(string folder)
        {
            string[] files = Directory.GetFiles(folder);
            files.ToList().ForEach(File.Delete);
        }

        // Generate error data for each file in the specified folder using a provided error-generating function.
        public static void CreateErrorData(string folder, Func<string, string> errorGenerator)
        {
            CleanFolder(folder); // Clean the specified folder before creating error data.

            // Get all file paths from the Clean folder.
            string[] files = Directory.GetFiles(Clean);

            // Loop through each file in the Clean folder.
            for (int i = 0; i < files.Length; i++)
            {
                string source = files[i]; // Get the file path.
                string name = Path.GetFileName(source); // Get the file name.

                // Read all lines from the source file.
                string[] lines = File.ReadAllLines(source);

                // Initialize a new list of lines starting with the header.
                List<string> newlines = new List<string> { lines[0] };

                // Apply the error generator function to each line (except the header).
                List<string> modifiedLines = lines.Skip(1).Select(errorGenerator).ToList();

                // Add modified lines to the new list.
                newlines.AddRange(modifiedLines);

                // Define the destination path for the new file.
                string destination = Path.Combine(folder, name);

                // Write the new lines to the destination file.
                File.WriteAllLines(destination, newlines);
            }
        }

        // Generate a random error percentage based on a predefined maximum error percentage.
        public static double ErrorPercentage() => Random.Shared.NextDouble() * ERRORPERCENTAGE;

        // Generate a mix of both subjective and objective errors
        public static string MixedError(string line)
        {
            string newLine = SubjectiveError(line);
            newLine = ObjectiveError(newLine);
            return newLine;
        }

        // Modify the data by introducing random objective errors into the trading data values (date, open, high, low, close, volume).
        public static string ObjectiveError(string line)
        {
            // Split the line by commas and remove any trailing/leading white-spaces from each value.
            string[] values = line.Split(',').Select(v => v.Trim()).ToArray();

            // Parse the date and numeric values.
            DateTime date = DateTime.Parse(values[0], CultureInfo.InvariantCulture);
            double open = double.Parse(values[1], CultureInfo.InvariantCulture);
            double high = double.Parse(values[2], CultureInfo.InvariantCulture);
            double low = double.Parse(values[3], CultureInfo.InvariantCulture);
            double close = double.Parse(values[4], CultureInfo.InvariantCulture);
            double volume = double.Parse(values[5], CultureInfo.InvariantCulture);

            // Introduce errors randomly based on conditions.
            if (AddError()) date = date.AddDays(Random.Shared.Next(1, 365)); // Randomly alter the date.
            if (AddError()) open = Random.Shared.Next(2) == 1 ? high * (1 + ErrorPercentage()) : low * (1 - ErrorPercentage()); // Modify the open price.
            if (AddError()) high = Random.Shared.Next(2) == 1 ? open * (1 - ErrorPercentage()) : close * (1 - ErrorPercentage()); // Modify the high price.
            if (AddError()) low = Random.Shared.Next(2) == 1 ? open * (1 + ErrorPercentage()) : close * (1 + ErrorPercentage()); // Modify the low price.
            if (AddError()) close = Random.Shared.Next(2) == 1 ? high * (1 + ErrorPercentage()) : low * (1 - ErrorPercentage()); // Modify the close price.
            if (AddError()) volume = -volume; // Randomly negate the volume.

            // Create a new line formatted with the modified values.
            string newLine = $"{date.ToString(CultureInfo.InvariantCulture)},{open:0.00},{high:0.00},{low:0.00},{close:0.00},{volume:0}";

            return newLine; // Return the modified line.
        }

        // Modify the data by introducing random subjective errors into the trading data values (date, open, high, low, close, volume) by modifying the values based on random fluctuations.
        public static string SubjectiveError(string line)
        {
            // Split the line by commas and trim whitespace from each value.
            string[] values = line.Split(',').Select(v => v.Trim()).ToArray();

            // Parse the date and numeric values from the file
            DateTime date = DateTime.Parse(values[0], CultureInfo.InvariantCulture);
            double open = double.Parse(values[1], CultureInfo.InvariantCulture);
            double high = double.Parse(values[2], CultureInfo.InvariantCulture);
            double low = double.Parse(values[3], CultureInfo.InvariantCulture);
            double close = double.Parse(values[4], CultureInfo.InvariantCulture);
            double volume = double.Parse(values[5], CultureInfo.InvariantCulture);

            // Introduce subjective errors randomly based on conditions.
            if (AddError()) open = Random.Shared.Next(2) == 1 ? open + (high - open) * Random.Shared.NextDouble() : open + (low - open) * Random.Shared.NextDouble(); // Modify the open price.
            if (AddError()) high = Random.Shared.Next(2) == 1 ? high + (high - open) * Random.Shared.NextDouble() : high + (high - close) * Random.Shared.NextDouble(); // Modify the high price.
            if (AddError()) low = Random.Shared.Next(2) == 1 ? low + (low - open) * Random.Shared.NextDouble() : low + (low - close) * Random.Shared.NextDouble(); // Modify the low price.
            if (AddError()) close = Random.Shared.Next(2) == 1 ? close + (high - close) * Random.Shared.NextDouble() : close + (low - close) * Random.Shared.NextDouble(); // Modify the close price.
            if (AddError()) volume = Exp(Log(volume) + (2 * Random.Shared.NextDouble() - 1) * ErrorPercentage()); // Introduce variability into the volume.

            // Create a new line formatted with the modified values.
            string newLine = $"{date.ToString(CultureInfo.InvariantCulture)},{open:0.00},{high:0.00},{low:0.00},{close:0.00},{volume:0}";

            return newLine; // Return the modified line.
        }
    }
}
