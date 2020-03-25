using System;

namespace Utilities.ValueIntervals
{
    public enum RangeType
    {
        OpenOpen,
        CloseClose,
        OpenClose,
        CloseOpen
    }
    public abstract class ValueInterval
    {
        protected ValueInterval(double max, double min, RangeType type)
        {
            if (max < min)
                throw new MaxNotBiggerThanMin($"最大值{max}不大于最小值{min}");

            Max = max;
            Min = min;
            Type = type;
        }

        public bool AllowOutRangeMap { get; set; } = true;
        protected ValueInterval() { }
        public double Max { get; private set; } = 0;
        public double Min { get; private set; } = 0;
        public RangeType Type { get; set; }
        public double Coverage => NumericDistance(Min, Max);
        public static double NumericDistance(double min, double max) => Math.Abs(max - min);
        public double NumericDistanceToMin(double value)
        {
            if (!AllowOutRangeMap)
            {
                if (!IsInRange(value))
                    throw new ValueNotInRange($"转换的数值{value}超出了范围:{ToString()}");
            }
            return NumericDistance(Min, value);
        }
        public abstract bool IsInRange(double value);
        public static ValueInterval OpenOpen(double max = 1, double min = 0, bool allowOutRangeMap = true) => new OpenOpenInterval() { Max = max, Min = min, Type = RangeType.OpenOpen, AllowOutRangeMap = allowOutRangeMap };
        public static ValueInterval OpenClose(double max = 1, double min = 0, bool allowOutRangeMap = true) => new OpenCloseInterval() { Max = max, Min = min, Type = RangeType.OpenClose, AllowOutRangeMap = allowOutRangeMap };
        public static ValueInterval CloseClose(double max = 1, double min = 0, bool allowOutRangeMap = true) => new CloseCloseInterval() { Max = max, Min = min, Type = RangeType.CloseClose, AllowOutRangeMap = allowOutRangeMap };
        public static ValueInterval CloseOpen(double max = 1, double min = 0, bool allowOutRangeMap = true) => new CloseOpenInterval() { Max = max, Min = min, Type = RangeType.CloseOpen, AllowOutRangeMap = allowOutRangeMap };
    }

    class OpenOpenInterval : ValueInterval
    {
        public override bool IsInRange(double value) => value > Min && value < Max;

        public override string ToString() => $"({Min}, {Max})";
    }

    class CloseCloseInterval : ValueInterval
    {
        public override bool IsInRange(double value) => value >= Min && value <= Max;

        public override string ToString() => $"[{Min}, {Max}]";
    }

    class OpenCloseInterval : ValueInterval
    {
        public override bool IsInRange(double value) => value > Min && value <= Max;

        public override string ToString() => $"({Min}, {Max}]";
    }

    class CloseOpenInterval : ValueInterval
    {
        public override bool IsInRange(double value) => value >= Min && value < Max;

        public override string ToString() => $"[{Min}, {Max})";
    }
}
