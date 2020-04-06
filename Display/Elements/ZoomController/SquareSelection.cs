using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Drawing;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.Display
{
    public class SquareSelection : SelectStrategy
    {
        public override Rectangle CalRect(Point centerPoint, Point cornerPoint)
        {
            var raduas = DistanceBetween(centerPoint, cornerPoint);
            Rectangle rect = new Rectangle(centerPoint.X - (int)raduas, centerPoint.Y - (int)raduas, (int)raduas * 2, (int)raduas * 2);
            return rect;
        }

        public override void DrawZoomView(RectF coverRect, RenderTarget rt, Brush fillBrush, Brush frameBrush, float strokeWidth)
        {
            Ellipse e = new Ellipse(coverRect.Center(), coverRect.Width / 2, coverRect.Width / 2);
            rt.FillEllipse(e, fillBrush);
            rt.DrawEllipse(e, frameBrush, strokeWidth);
        }

        private double DistanceBetween(Point p1, Point p2) => Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
    }
}
