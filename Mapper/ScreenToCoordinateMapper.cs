using System;
using System.Drawing;

namespace Utilities.Mapper
{
    public class ScreenToCoordinateMapper : IScreenToCoordinateMapper
    {
        //public ValueMapper XAxisMapper { get; protected set; } = new ValueMapper();
        //public ValueMapper YAxisMapper { get; protected set; } = new ValueMapper();
        private ValueMapper XAxisMapper { get;  set; } = new ValueMapper();
        private ValueMapper YAxisMapper { get;  set; } = new ValueMapper();

        public double ScreenLeft => XAxisMapper.Value1Left;

        public double ScreenRight => XAxisMapper.Value1Right;

        public double ScreenTop => YAxisMapper.Value1Left;  //?????

        public double ScreenBottom => YAxisMapper.Value1Right;

        public double CoordinateLeft => XAxisMapper.Value2Left;

        public double CoordinateRight => XAxisMapper.Value2Right;

        public double CoordinateTop => YAxisMapper.Value2Left;  //?????

        public double CoordinateBottom => YAxisMapper.Value2Right;

        public PointF ScreenCenter => new PointF(Math.Abs((float)ScreenLeft + (float)ScreenRight) / 2, Math.Abs((float)ScreenTop + (float)ScreenBottom) / 2);

        public double ScreenWidth => Math.Abs(ScreenRight - ScreenLeft);

        public double ScreenHeight => Math.Abs(ScreenTop - ScreenBottom);

        //public ScreenToCoordinateMapper(ValueMapper xAxis, ValueMapper yAxis)
        //{
        //    XAxisMapper = xAxis ?? throw new Exception("映射器为null");
        //    YAxisMapper = yAxis ?? throw new Exception("映射器为null");
        //}
        public ScreenToCoordinateMapper(double screenLeft, double screenRight, double coordinateXLeft, double coordinateXRight, double screenTop, double screenBottom, double coordinateYTop, double coordinateYBottom)
        {
            //XAxisMapper = new ValueMapper(screenLeft, screenRight, coordinateXLeft, coordinateXRight);
            //YAxisMapper = new ValueMapper(screenTop, screenBottom, coordinateYTop, coordinateYBottom);
            SetCoordinateXRange(coordinateXLeft, coordinateXRight);
            SetCoordinateYRange(coordinateYTop, coordinateYBottom);
            SetScreenArea(screenLeft, screenRight, screenTop, screenBottom);
        }

        public ScreenToCoordinateMapper() : this(screenLeft: 0, screenRight: 1, coordinateXLeft: 0, coordinateXRight: 1, screenTop: 0, screenBottom: 1, coordinateYTop: 1, coordinateYBottom: 0)
        { }
        public double GetScreenX(double coordinateX) => XAxisMapper.MapToValue1(coordinateX);
        public double GetScreenY(double coordinateY) => YAxisMapper.MapToValue1(coordinateY);
        public double GetCoordinateX(double screenX) => XAxisMapper.MapToValue2(screenX);
        public double GetCoordinateY(double screenY) => YAxisMapper.MapToValue2(screenY);
        public PointF GetScreenLocation(double coordinateX, double coordinateY) => new PointF((int)GetScreenX(coordinateX), (int)GetScreenY(coordinateY));
        public PointF GetCoordinateLocation(double screenX, double screenY) => new PointF((int)GetCoordinateX(screenX), (int)GetCoordinateY(screenY));
        public void SetCoordinateXRange(double xLeft, double xRight) => XAxisMapper.SetRange2(xLeft, xRight);
        public void SetCoordinateYRange(double yTop, double yBottom) => YAxisMapper.SetRange2(yTop, yBottom);
        public void SetScreenArea(double left, double right, double top, double bottom)
        {
            XAxisMapper.SetRange1(left, right);
            YAxisMapper.SetRange1(top, bottom);
        }
    }
}
