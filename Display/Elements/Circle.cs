using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Drawing;
using Utilities.Tools;

namespace Utilities.Display
{
    public class Circle : MouseSensitiveObject
    {
        public Circle(Ellipse ellipse)
        {
            Ellipse = ellipse;
        }
        public Ellipse Ellipse { get; set; }
        public PointF TextLeftTop { get; set; }

        public double DistanceToCirleEdge(PointF p)
        {
            var disToCenter = Functions.DistanceBetween(p.ToPoint2F(), Ellipse.Point);
            return Math.Abs(Ellipse.RadiusX - disToCenter);
        }

        public override bool IsMouseNear(PointF mouseLocation) => DistanceToCirleEdge(mouseLocation) < 6;
    }
}
