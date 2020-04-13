using System;
using System.Drawing;
using Utilities.Mapper;

namespace Utilities.Display
{
    public class ReferenceSystem
    {
        public ReferenceSystem(double left, double right, double top, double bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        public PointF ScreenOriginalPoint => Mapper.GetScreenLocation(0, 0);
        public double Left { get; private set; }
        public double Right { get; private set; }
        public double Top { get; private set; }
        public double Bottom { get; private set; }

        public double ScreenLeft => Mapper.GetScreenX(Left);
        public double ScreenRight => Mapper.GetScreenX(Right);
        public double ScreenTop => Mapper.GetScreenY(Top);
        public double ScreenBottom => Mapper.GetScreenY(Bottom);

        public double ScreenWidth => ScreenRight - ScreenLeft;
        public double ScreenHeight => ScreenBottom - ScreenTop;

        public double XDistance => Math.Abs(Right - Left);
        public double YDistance => Math.Abs(Top - Bottom);
        public IScreenToCoordinateMapper Mapper { get; private set; }
        public void SetMapper(IScreenToCoordinateMapper mapper)
        {
            Mapper = mapper;
            Mapper.SetCoordinateArea(Left, Right, Top, Bottom);
        }
        public void SetArea(double left, double right, double top, double bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
            Mapper.SetCoordinateArea(left, right, top, bottom);
        }
    }
}
