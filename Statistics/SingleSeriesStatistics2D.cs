using System;
using System.Collections.Generic;

namespace Utilities.Statistics
{
    public enum StatisticsType
    {
        Sum,
        Count,
        Average,
        UserDefine
    }

    internal class SingleSeriesStatistics<TObject> where TObject : class
    {
        private readonly static Func<IEnumerable<double>, double> SumCalculator = p => 
        {
            double sum = 0;
            foreach (var value in p)
            {
                sum += value;
            }
            return sum;
        }; 
        private readonly static Func<IEnumerable<double>, double> CountCalculator = p => 
        {
            int count = 0;
            foreach (var value in p)
            {
                count++;
            }
            return count;
        };
        private readonly static Func<IEnumerable<double>, double> AverageCalculator = p => 
        {
            double sum = 0;
            int i = 0;
            foreach (var value in p)
            {
                sum += value;
                i++;
            }
            return sum / i;
        };

        private Func<TObject, string> XAxisMapper;
        private Func<TObject, double> YAxisMapper;

        public Func<IEnumerable<double>, double> foldMethod { get; set; }

        public SingleSeriesStatistics(Func<TObject, string> xAxisMapper , Func<TObject, double> yAxisMapper, 
            StatisticsType type = StatisticsType.Sum, Func<IEnumerable<double>, double> foldMethod = null)
        {
            switch (type)
            {
                case StatisticsType.Sum:
                    foldMethod = SumCalculator;
                    break;
                case StatisticsType.Count:
                    foldMethod = CountCalculator;
                    break;
                case StatisticsType.Average:
                    foldMethod = AverageCalculator;
                    break;
                case StatisticsType.UserDefine:
                    if (foldMethod == null) throw new Exception("传入UserDefine类型时，calculator不能为null");
                        this.foldMethod = foldMethod;
                    break;
                default:
                    throw new Exception($"错误的StatisticsType类型{type}");
            }

            XAxisMapper = xAxisMapper;
            YAxisMapper = yAxisMapper;
        }
        public Dictionary<string, double> Statistics(IEnumerable<TObject> objects)
        {
            if (objects == null) return new Dictionary<string, double>();

            var seriesValues = GetSeriesNumbers(objects);
            var ret = new Dictionary<string, double>();
            foreach (var item in seriesValues)
            {
                ret.Add(item.Key, foldMethod.Invoke(item.Value));
            }

            return ret;
        }
        private Dictionary<string, List<double>> GetSeriesNumbers(IEnumerable<TObject> objects)
        {
            Dictionary<string, List<double>> ret = new Dictionary<string, List<double>>();
            foreach (var item in objects)
            {
                string xAxisValue = XAxisMapper.Invoke(item);
                double seriesValue = YAxisMapper.Invoke(item);

                if (!ret.ContainsKey(xAxisValue))
                    ret.Add(xAxisValue, new List<double>());
                ret[xAxisValue].Add(seriesValue);
            }
            return ret;
        }
    }
}
