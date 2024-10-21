using System.Globalization;
using static System.Math;
using Values = (System.DateTime Date, double Open, double High, double Low, double Close, double Volume);

namespace UnitTest
{
    public static class Functions
    {
        public static bool AllFilesReadable(string folder)
        {
            string[] files = Directory.GetFiles(folder);
            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    string source = files[i];
                    string[] lines = File.ReadAllLines(source).Skip(1).ToArray();
                    List<Values> valueList = lines.Select(Values).ToList();
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        internal static double CumulativeSquareError(string folder1, string folder2)
        {
            double cse = 0;
            List<string> files1 = Directory.GetFiles(folder1).Select(f => Path.GetFileName(f)).ToList();
            List<string> files2 = Directory.GetFiles(folder2).Select(f => Path.GetFileName(f)).ToList();
            List<string> files = files1.Intersect(files2).ToList();
            for (int i = 0; i < files.Count; i++)
            {
                string file1 = Path.Combine(folder1, files1[i]);
                List<Values> values1 = GetValues(file1);
                string file2 = Path.Combine(folder2, files2[i]);
                List<Values> values2 = GetValues(file2);
                cse += HistoricalSeriesSquareDistance(values1, values2);
            }
            return cse;
        }

        private static double HistoricalSeriesSquareDistance(List<Values> values1, List<Values> values2)
        {
            double hse = 0;
            List<DateTime> dates = values1.Select(v => v.Date).Intersect(values2.Select(v => v.Date)).ToList();
            values1.RemoveAll(v => !dates.Contains(v.Date));
            values2.RemoveAll(v => !dates.Contains(v.Date));
            for (int i = 0; i < dates.Count; i++)
            {
                Values v1 = values1[i];
                Values v2 = values2[i];
                hse += Pow(Log(v1.Open / v2.Open), 2);
                hse += Pow(Log(v1.High / v2.High), 2);
                hse += Pow(Log(v1.Low / v2.Low), 2);
                hse += Pow(Log(v1.Close / v2.Close), 2);
            }
            return hse;
        }

        public static List<Values> GetValues(string file)
        {
            string[] lines = File.ReadAllLines(file).Skip(1).ToArray();
            List<Values> valueList = lines.Select(Values).ToList();
            return valueList;
        }

        public static bool NoNAN(string folder)
        {
            string[] files = Directory.GetFiles(folder);
            for (int i = 0; i < files.Length; i++)
            {
                string source = files[i];
                string[] lines = File.ReadAllLines(source).Skip(1).ToArray();
                List<Values> valueList = lines.Select(Values).ToList();
                if (valueList.SelectMany(v => new double[] { v.Open, v.High, v.Low, v.Close }).Any(double.IsNaN)) return false;
            }
            return true;
        }

        private static bool IsOHLCConsistent(Values values)
        {
            if (values.Open > values.High || values.Low > values.High || values.Close > values.High) return false;
            if (values.Open < values.Low || values.High < values.Low || values.Close < values.Low) return false;
            return true;
        }

        public static bool OHLCConsistent(string folder)
        {
            string[] files = Directory.GetFiles(folder);
            for (int i = 0; i < files.Length; i++)
            {
                string source = files[i];
                string[] lines = File.ReadAllLines(source).Skip(1).ToArray();
                List<Values> valueList = lines.Select(Values).ToList();
                if (valueList.Any(v => !IsOHLCConsistent(v))) return false;
            }
            return true;
        }

        public static bool OHLCPositive(string folder)
        {
            string[] files = Directory.GetFiles(folder);
            for (int i = 0; i < files.Length; i++)
            {
                string source = files[i];
                string[] lines = File.ReadAllLines(source).Skip(1).ToArray();
                List<Values> valueList = lines.Select(Values).ToList();
                if (valueList.SelectMany(v => new double[] { v.Open, v.High, v.Low, v.Close }).Any(v => v < 0)) return false;
            }
            return true;
        }

        public static bool PastDates(string folder)
        {
            string[] files = Directory.GetFiles(folder);
            for (int i = 0; i < files.Length; i++)
            {
                string source = files[i];
                string[] lines = File.ReadAllLines(source).Skip(1).ToArray();
                List<Values> valueList = lines.Select(Values).ToList();
                if (valueList.Any(v => v.Date > DateTime.Now)) return false;
            }
            return true;
        }

        public static bool PositiveVolumes(string folder)
        {
            string[] files = Directory.GetFiles(folder);
            for (int i = 0; i < files.Length; i++)
            {
                string source = files[i];
                string[] lines = File.ReadAllLines(source).Skip(1).ToArray();
                List<Values> valueList = lines.Select(Values).ToList();
                if (valueList.Any(v => v.Volume < 0)) return false;
            }
            return true;
        }

        public static bool ValidTradingDays(string folder)
        {
            string[] files = Directory.GetFiles(folder);
            for (int i = 0; i < files.Length; i++)
            {
                string source = files[i];
                string[] lines = File.ReadAllLines(source).Skip(1).ToArray();
                List<Values> valueList = lines.Select(Values).ToList();
                if (valueList.Any(v => v.Date.DayOfWeek == DayOfWeek.Saturday || v.Date.DayOfWeek == DayOfWeek.Sunday)) return false;
            }
            return true;
        }

        public static Values Values(string line)
        {
            string[] values = line.Split(',').Select(v => v.Trim()).ToArray();
            DateTime date = DateTime.Parse(values[0], CultureInfo.InvariantCulture);
            double open = double.Parse(values[1], CultureInfo.InvariantCulture);
            double high = double.Parse(values[2], CultureInfo.InvariantCulture);
            double low = double.Parse(values[3], CultureInfo.InvariantCulture);
            double close = double.Parse(values[4], CultureInfo.InvariantCulture);
            double volume = double.Parse(values[5], CultureInfo.InvariantCulture);
            return (date, open, high, low, close, volume);
        }
    }
}
