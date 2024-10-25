using System.Globalization;

namespace Models
{
    // This class includes properties that reflect ones acquired from financial data. It also includes methods to load and save datasets.
    public class Quote
    {
        public const string HEADER = "Date,Open,High,Low,Close,Volume";

        public readonly DateTime Date;
        public readonly double Open;
        public readonly double High;
        public readonly double Low;
        public readonly double Close;
        public readonly double Volume;
        public readonly double Average;
        public readonly double Liquidity; // The ease to trade without affecting the asset's price

        /* Represent a financial quote with associated date, OHLC (Open, High, Low, Close) prices, volume, average price, and liquidity.
           Validate the input values to ensure they meet specific criteria for a valid trading quote. */
        public Quote(DateTime date, double open, double high, double low, double close, double volume)
        {
            // Initialize an array of doubles with the given values
            double[] values = { open, high, low, close, volume };

            // Validate that the date is not in the future.
            if (date > DateTime.Now) throw new ArgumentException("Date is in the future.");

            // Validate that the date is a valid trading day (not Saturday or Sunday).
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                throw new ArgumentException("Date is not a valid trading day.");

            // Check that no values are NaN.
            if (values.Any(double.IsNaN)) throw new ArgumentException("NaN values are not acceptable in a quote.");

            // Validate that the volume is not negative.
            if (volume < 0) throw new ArgumentException("Volume is negative.");

            // Ensure that all OHLC values are positive.
            if (values.Take(4).Any(v => v <= 0)) throw new ArgumentException("OHLC values must all be positive.");

            // Check for consistency in OHLC values.
            if (open > high || low > high || close > high || open < low || high < low || close < low)
                throw new ArgumentException("OHLC values are inconsistent.");

            // Assign validated values to properties.
            Date = date;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;

            // Calculate the average price and liquidity based on OHLC values and volume.
            Average = (Open + High + Low + Close) / 4;
            Liquidity = Average * Volume;
        }

        // Load a list of financial quotes from a specified CSV file path.
        // Parse each line to create Quote objects, skipping empty lines and handling any parsing errors.
        public static List<Quote> Load(string path)
        {
            List<Quote> quotes = new List<Quote>();

            // Read all lines from the file and filter out any empty lines.
            List<string> lines = File.ReadAllLines(path).Where(l => !string.IsNullOrEmpty(l)).ToList();

            // Iterate through each line, starting from the second line (index 1), to skip the headers
            for (int i = 1; i < lines.Count; i++)
            {
                try
                {
                    // Split the line by commas into individual values and remove trailing/leading white-spaces.
                    string[] vals = lines[i].Split(',').Select(v => v.Trim()).ToArray();

                    // Parse each value into the appropriate data type.
                    DateTime date = DateTime.Parse(vals[0], CultureInfo.InvariantCulture);
                    double open = double.Parse(vals[1], CultureInfo.InvariantCulture);
                    double high = double.Parse(vals[2], CultureInfo.InvariantCulture);
                    double low = double.Parse(vals[3], CultureInfo.InvariantCulture);
                    double close = double.Parse(vals[4], CultureInfo.InvariantCulture);
                    double volume = double.Parse(vals[5], CultureInfo.InvariantCulture);

                    // Create a new Quote object and add it to the list.
                    Quote q = new Quote(date, open, high, low, close, volume);
                    quotes.Add(q);
                }
                catch
                {
                    // Skip any lines that cause parsing errors.
                }
            }
            // Return the list of parsed quotes.
            return quotes;
        }

        // Save data from a list into a file at the specified path
        public static void Save(string path, List<Quote> quotes)
        {
            File.WriteAllText(path, HEADER + "\n");
            File.AppendAllLines(path, quotes.Select(q => q.ToString()));
        }

        // Output a string displaying data for each date in the given format
        public override string ToString()
        {
            return $"{Date.ToString(CultureInfo.InvariantCulture)},{Open:0.00},{High:0.00},{Low:0.00},{Close:0.00},{Volume:0}";
        }
    }
}
