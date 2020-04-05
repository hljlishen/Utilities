using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Drawing;
using Utilities.Coordinates;
using Utilities.Tools;

namespace Utilities.Display
{
    public class PpiAntenna : DynamicElement<double>
    {
        private double degree = 0;
        public Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush AntennaPen = null;
        protected override void DrawDynamicElement(RenderTarget g)
        {
            RectangularCoordinate p = new PolarCoordinate(degree, 0, (Background as PpiBackground).Range).Rectangular;
            PointF endPoint = Mapper.GetScreenLocation(p.X, p.Y);
            PointF beginPoint = Mapper.GetScreenLocation(0, 0);
                
            g.DrawLine(new Point2F(beginPoint.X, beginPoint.Y), new Point2F(endPoint.X, endPoint.Y), AntennaPen, 2);
        }

        protected override void DoUpdate(double t)
        {
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
