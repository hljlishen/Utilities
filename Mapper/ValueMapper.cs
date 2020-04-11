using System;
using Utilities.ValueIntervals;
namespace Utilities.Mapper
{
    public class ValueMapper
    {
        public ValueMapper(double value1Left, double value1Right, double value2Left, double value2Right)
        {
            Value1Left = value1Left;
            Value2Left = value2Left;
            Value1Right = value1Right;
            Value2Right = value2Right;
            //如果两组数据的left-->right的方向不一样，则作对称映射
            //不用考虑相等的情况，如果left和right相等，new ValueRange会抛出异常
            v1Direction = value1Right > value1Left;
            v2Direction = value2Right > value2Left;
            reverseMap = IsSameDirection();

            Range1 = ValueInterval.CloseClose(Math.Min(value1Right, value1Left), Math.Max(value1Right, value1Left));
            Range2 = ValueInterval.CloseClose(Math.Min(value2Right, value2Left), Math.Max(value2Right, value2Left));
            CalculateRatos();
        }

        public ValueMapper() : this(-1, 1, -1, 1)
        { }
        public ValueInterval Range1 { get; private set; }
        public void SetRange1(double left, double right)
        {
            AssertValidRange(left, right);
            Value1Left = left;
            Value1Right = right;
            v1Direction = right > left;
            reverseMap = IsSameDirection();
            Range1 = ValueInterval.CloseClose(Math.Min(right, left), Math.Max(right, left));
            CalculateRatos();
        }
        private static void AssertValidRange(double left, double right)
        {
            if (IsIntervalTooSmall(left, right))
                throw new Exception($"{left}--{right}过于接近，无法形成取值范围");
        }

        public static bool IsIntervalTooSmall(double left, double right) => Math.Abs(left - right) < 0.00001;
        public ValueInterval Range2 { get; private set; }
        public void SetRange2(double left, double right)
        {
            AssertValidRange(left, right);
            Value2Left = left;
            Value2Right = right;
            v2Direction = right > left;
            reverseMap = IsSameDirection();
            Range2 = ValueInterval.CloseClose(Math.Min(right, left), Math.Max(right, left));
            CalculateRatos();
        }
        private bool IsSameDirection()
        {
            if ((v1Direction == true && v2Direction == true) || (v1Direction == false && v2Direction == false))
                return false;
            else
                return true;
        }

        private bool v1Direction;
        private bool v2Direction;
        private void CalculateRatos()
        {
            Value2ToValue1Rato = Range2.Coverage / Range1.Coverage;
            Value1ToValue2Rato = Range1.Coverage / Range2.Coverage;
        }

        private double Value2ToValue1Rato;
        private double Value1ToValue2Rato;
        public double Value1Left { get; private set; }
        public double Value1Right { get; private set; }
        public double Value2Left { get; private set; }
        public double Value2Right { get; private set; }
        private bool reverseMap = false;

        public double MapToValue1(double value2)
        {
            var value2Dis = Range2.NumericDistanceToMin(value2);
            var value1Dis = value2Dis * Value1ToValue2Rato;
            if (reverseMap)
                return Range1.Max - value1Dis;
            else
                return value1Dis + Range1.Min;
        }
        public double MapToValue2(double value1)
        {
            var value1Dis = Range1.NumericDistanceToMin(value1);
            var value2Dis = value1Dis * Value2ToValue1Rato;
            if (reverseMap)
                return Range2.Max - value2Dis;
            else
                return value2Dis + Range2.Min;
        }
    }
}
