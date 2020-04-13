using System;
using System.Drawing;

namespace Utilities.Mapper
{
    public class SquaredScreenRectDecorator : MapperDecorator
    {
        public SquaredScreenRectDecorator(IScreenToCoordinateMapper mapper) : base(mapper)
        {
        }
        public override void SetScreenArea(double left, double right, double top, double bottom)
        {
            var area = FindSquareInArea(left, right, top, bottom);
            base.SetScreenArea(area.Left, area.Right, area.Top, area.Bottom);
        }

        public static PointF CenterPointOf(RectangleF rect) => new PointF((rect.Right + rect.Left) / 2, (rect.Bottom + rect.Top) / 2);

        public static PointF CenterPoint(double left, double right, double top, double bottom) => new PointF(Math.Abs((float)left + (float)right) / 2, Math.Abs((float)top + (float)bottom) / 2);
        public static double MaximumSquareWidth(double left, double right, double top, double bottom) => Math.Min(Math.Abs(left - right), Math.Abs(top - bottom)) / 2;

        private Area FindSquareInArea(double left, double right, double top, double bottom)
        {
            var center = CenterPoint(left, right, top, bottom);
            var width = MaximumSquareWidth(left, right, top, bottom);
            bool isXIncreasing = right > left;
            bool isYIncreaseing = top > bottom;

            Area ret;

            if (isXIncreasing)
            {
                ret.Left = center.X - width;
                ret.Right = center.X + width;
            }
            else
            {
                ret.Left = center.X + width;
                ret.Right = center.X - width;
            }

            if (isYIncreaseing)
            {
                ret.Top = center.Y + width;
                ret.Bottom = center.Y - width;
            }
            else
            {
                ret.Top = center.Y - width;
                ret.Bottom = center.Y + width;
            }

            return ret;
        }
    }
}
