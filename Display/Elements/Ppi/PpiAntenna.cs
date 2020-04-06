using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Drawing;
using Utilities.Coordinates;

namespace Utilities.Display
{
    public class PpiAntenna : DynamicElement<double>
    {
        private double preDegree = 0;
        private double degree = 0;
        private int shadeLen = 200;
        private double shadeStep = 0.1f;
        private Color antennaColor = Color.White;
        public Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush antennaBrush = null;
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
                shadeLen = 1;
            for (int i = 0; i < shadeLen; i++)
            {
                RectangularCoordinate p = new PolarCoordinate(degree + shadeStep * i, 0, ReferenceSystem.Right).Rectangular;
                PointF endPoint = Mapper.GetScreenLocation(p.X, p.Y);
                PointF beginPoint = Mapper.GetScreenLocation(0, 0);

                antennaBrush.Opacity = 1 - i * opStep;
                g.DrawLine(new Point2F(beginPoint.X, beginPoint.Y), new Point2F(endPoint.X, endPoint.Y), antennaBrush, 2);
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
            antennaBrush?.Dispose();
        }

        protected override void InitializeComponents(RenderTarget rt)
        {
            antennaBrush = antennaColor.SolidBrush(rt);
        }
    }
}
