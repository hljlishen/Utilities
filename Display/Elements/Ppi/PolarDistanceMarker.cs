using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Utilities.Display
{
    public class PolarDistanceMarker : MarkerElement<LiveCircle>
    {
        public PolarDistanceMarker(MarkerModel model) : base(model)
        {
        }

        public PolarDistanceMarker() : this(new MarkerModel() { LineColor = Color.Green, LineWidth = 2, ObjectNumber = 5, SelectedLineColor = Color.Orange, SelectedLineWidth = 4, FontName = "Berlin Sans FB Demi", FontColor = Color.Gray, FontSize = 15, SelectedFontColor = Color.Orange, SelectedFontName = "Berlin Sans FB Demi", SelectedFontSize = 15 }) { }

        protected override IEnumerable<LiveCircle> GetObjects()
        {
            var step = Math.Abs(ReferenceSystem.Top) / Model.ObjectNumber;
            var center = ReferenceSystem.ScreenOriginalPoint;
            for (int i = 0; i < Model.ObjectNumber + 1; i++)
            {
                var r = Math.Abs(Mapper.GetScreenY(step * i) - center.Y);
                yield return new LiveCircle(new Ellipse(center.ToPoint2F(), (float)r, (float)r)) { Value = step * i, TextLeftTop = new PointF(center.X, center.Y - (float)r) };
            }
        }

        protected override void DrawObjectUnselected(RenderTarget rt, LiveCircle c)
        {
            rt.DrawEllipse(c.Ellipse, normalLineBrush, Model.LineWidth);
            rt.DrawText(((double)c.Value).ToString("0"), normalTextFormat, new RectF(c.TextLeftTop.X + 3, c.TextLeftTop.Y, c.TextLeftTop.X + 100, c.TextLeftTop.Y + 100), normalTextBrush);
        }

        protected override void DrawObjectSelected(RenderTarget rt, LiveCircle c)
        {
            rt.DrawEllipse(c.Ellipse, selectedLineBrush, Model.SelectedLineWidth);
            rt.DrawText(((double)c.Value).ToString("0.0"), selectedTextFormat, new RectF(c.MouseLocation.X + 10, c.MouseLocation.Y, 1000, 1000), selectedTextBrush);

            rt.DrawText(((double)c.Value).ToString("0"), normalTextFormat, new RectF(c.TextLeftTop.X + 3, c.TextLeftTop.Y, c.TextLeftTop.X + 100, c.TextLeftTop.Y + 100), normalTextBrush);
        }
    }
}
