using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Drawing;
using Utilities.Coordinates;
using Utilities.Tools;

namespace Utilities.Display
{
    public class PpiAntenna : DynamicElement<double>
    {
        private double preDegree = 0;
        private double degree = 0;
        private int shadeLen = 200;
        private double shadeStep = 0.1f;
        public Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush AntennaPen = null;
        protected override void DrawDynamicElement(RenderTarget g)
        {
            var opStep = 1.0f / shadeLen;
            if (preDegree > degree)
            {
                shadeLen = 200;
                shadeStep = 0.1f;
            }
            else if (preDegree < degree)
            {
                shadeStep = -0.1f;
                shadeLen = 200;
            }
            else
                shadeStep = 1;
            for (int i = 0; i < shadeLen; i++)
            {
                RectangularCoordinate p = new PolarCoordinate(degree + shadeStep * i, 0, (Background as PpiBackground).Range).Rectangular;
                PointF endPoint = Mapper.GetScreenLocation(p.X, p.Y);
                PointF beginPoint = Mapper.GetScreenLocation(0, 0);

                AntennaPen.Opacity = 1 - i * opStep;
                g.DrawLine(new Point2F(beginPoint.X, beginPoint.Y), new Point2F(endPoint.X, endPoint.Y), AntennaPen, 2);
            }
        }

        private void DrawShade(RenderTarget g, double angle, double step, int length)
        {
            for (int i = 0; i < length; i++)
            {
                RectangularCoordinate p = new PolarCoordinate(degree, 0, (Background as PpiBackground).Range).Rectangular;
            }
        }

        protected override void DoUpdate(double t)
        {
            preDegree = degree;
            degree = t;
        }

        public override void Dispose()
        {
            base.Dispose();
            AntennaPen?.Dispose();
        }

        protected override void InitializeComponents(RenderTarget rt)
        {
            AntennaPen = rt.CreateSolidColorBrush(Functions.GetColorFFromRgb(255, 255, 255));
        }
    }
}
