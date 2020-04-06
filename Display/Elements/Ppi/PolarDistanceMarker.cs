using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Utilities.Display
{
    public class PolarDistanceMarker : MouseSensitiveElement<Circle>
    {
        public PolarDistanceMarker(MarkerModel model) : base(model)
        {
        }

        protected override void DrawDynamicElement(RenderTarget rt)
        {
            foreach (var o in objects)
            {
                var c = o as Circle;
                if (c.Selected)
                {
                    rt.DrawEllipse(c.Ellipse, selectedLineBrush, Model.SelectedLineWidth);
                    rt.DrawText(c.Value.ToString("0.0"), selectedTextFormat, new RectF(c.MouseLocation.X + 10, c.MouseLocation.Y, 1000, 1000), selectedTextBrush);
                }
                else
                    rt.DrawEllipse(c.Ellipse, normalLineBrush, Model.LineWidth);
                rt.DrawText(c.Value.ToString("0"), normalTextFormat, new RectF(c.TextLeftTop.X + 3, c.TextLeftTop.Y, c.TextLeftTop.X + 100, c.TextLeftTop.Y + 100), normalTextBrush);
            }
        }

        protected override IEnumerable<Circle> GetObjects()
        {
            var step = Math.Abs(ReferenceSystem.Top) / Model.ObjectNumber;
            var center = ReferenceSystem.ScreenOriginalPoint;
            for (int i = 0; i < Model.ObjectNumber + 1; i++)
            {
                var r = Math.Abs(Mapper.GetScreenY(step * i) - center.Y);
                yield return new Circle(new Ellipse(center.ToPoint2F(), (float)r, (float)r)) { Value = step * i, TextLeftTop = new PointF(center.X, center.Y - (float)r) };
            }
        }
    }
}
