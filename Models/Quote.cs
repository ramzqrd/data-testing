using System.Globalization;

namespace Models
{
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
        public readonly double Liquidity;

        public Quote(DateTime date, double open, double high, double low, double close, double volume)
        {
            double[] values = { open, high, low, close, volume };
            if (date > DateTime.Now) throw new ArgumentException("Date is in the future.");
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) throw new ArgumentException("Date is not a valid trading day.");
            if (values.Any(double.IsNaN)) throw new ArgumentException("NaN values are not acceptable in a quote.");
            if (volume < 0) throw new ArgumentException("Volume is negative.");
            if (values.Take(4).Any(v => v <= 0)) throw new ArgumentException("OHLC values must all be positive.");
            if (open > high || low > high || close > high || open < low || high < low || close < low) throw new ArgumentException("OHLC values are inconsistent.");
            Date = date;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
            Average = (Open + High + Low + Close) / 4;
            Liquidity = Average * Volume;
        }


        public static List<Quote> Load(string path)
        {
            List<Quote> quotes = new List<Quote>();
            List<string> lines = File.ReadAllLines(path).Where(l => !string.IsNullOrEmpty(l)).ToList();
            for (int i = 1; i < lines.Count; i++)
            {
                try
                {
                    string[] vals = lines[i].Split(',').Select(v => v.Trim()).ToArray();
                    DateTime date = DateTime.Parse(vals[0], CultureInfo.InvariantCulture);
                    double open = double.Parse(vals[1], CultureInfo.InvariantCulture);
                    double high = double.Parse(vals[2], CultureInfo.InvariantCulture);
                    double low = double.Parse(vals[3], CultureInfo.InvariantCulture);
                    double close = double.Parse(vals[4], CultureInfo.InvariantCulture);
                    double volume = double.Parse(vals[5], CultureInfo.InvariantCulture);
                    Quote q = new Quote(date, open, high, low, close, volume);
                    quotes.Add(q);
                }
                catch { }
            }
            return quotes;
        }

        public static void Save(string path, List<Quote> quotes)
        {
            File.WriteAllText(path, HEADER + "\n");
            File.AppendAllLines(path, quotes.Select(q => q.ToString()));
        }

        public override string ToString()
        {
            return $"{Date.ToString(CultureInfo.InvariantCulture)},{Open:0.00},{High:0.00},{Low:0.00},{Close:0.00},{Volume:0}";
        }
    }
}
