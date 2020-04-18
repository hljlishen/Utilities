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

        public double Left { get; private set; }
        public double Right { get; private set; }
        public double Top { get; private set; }
        public double Bottom { get; private set; }

        public PointF ScreenOriginalPoint { get; private set; }/* => Mapper.GetScreenLocation(0, 0);*/
        public double ScreenLeft { get; private set; } /*=> Mapper.GetScreenX(Left);*/
        public double ScreenRight { get; private set; } /*=> Mapper.GetScreenX(Right);*/
        public double ScreenTop { get; private set; } /*=> Mapper.GetScreenY(Top);*/
        public double ScreenBottom { get; private set; } /*=> Mapper.GetScreenY(Bottom);*/
        public double ScreenWidth { get; private set; } /*=> ScreenRight - ScreenLeft;*/
        public double ScreenHeight { get; private set; } /*=> ScreenBottom - ScreenTop;*/
        public double XDistance  => Math.Abs(Right - Left);
        public double YDistance  => Math.Abs(Top - Bottom);
        public IScreenToCoordinateMapper Mapper { get; private set; }
        public void SetMapper(IScreenToCoordinateMapper mapper)
        {
            Mapper = mapper;
            Mapper.SetCoordinateArea(Left, Right, Top, Bottom);
            Mapper.MapperStateChanged += Mapper_MapperStateChanged;
        }

        private void Mapper_MapperStateChanged(IScreenToCoordinateMapper obj)
        {
           ScreenOriginalPoint = Mapper.GetScreenLocation(0, 0);
            ScreenLeft = Mapper.GetScreenX(Left);
            ScreenRight = Mapper.GetScreenX(Right);
            ScreenTop = Mapper.GetScreenX(Top);
            ScreenBottom = Mapper.GetScreenX(Bottom);
            ScreenHeight = Math.Abs(ScreenTop - ScreenBottom);
            ScreenWidth = Math.Abs(ScreenRight - ScreenLeft);
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
