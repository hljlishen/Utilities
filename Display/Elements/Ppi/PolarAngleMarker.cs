using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Drawing;
using Utilities.Tools;

namespace Utilities.Display
{
    public class PolarAngleMarker : MouseMoveElement<Line>
    {
        private StrokeStyle strokeStyle;

        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            strokeStyle = rt.Factory.CreateStrokeStyle(new StrokeStyleProperties { DashStyle = DashStyle.DashDot });
        }

        public override void Dispose()
        {
            base.Dispose();
            strokeStyle?.Dispose();
        }

        public PolarAngleMarker(MarkerModel model) : base(model)
        {
        }

        public PolarAngleMarker() : this(new MarkerModel() { LineColor = Color.White, LineWidth = 1, ObjectNumber = 12, SelectedLineColor = Color.Red, SelectedLineWidth = 3, FontName = "Berlin Sans FB Demi", FontColor = Color.Gray, FontSize = 15, SelectedFontColor = Color.Red, SelectedFontName = "Berlin Sans FB Demi", SelectedFontSize = 15 }) { }

        private IEnumerable<double> CalAngles(uint angleMarkerNumber)
        {
            double step = (double)360 / angleMarkerNumber;
            for (int i = 0; i < angleMarkerNumber; i++)
            {
                yield return step * i;
            }
        }

        protected override void DrawDynamicElement(RenderTarget rt)
        {
            foreach (var o in objects)
            {
                var l = o as Line;
                if (l.Selected)
                {
                    rt.DrawLine(l.P1.ToPoint2F(), l.P2.ToPoint2F(), selectedLineBrush, Model.SelectedLineWidth, strokeStyle);
                    rt.DrawText(l.Value.ToString("0") + "°", selectedTextFormat, new RectangleF(l.MouseLocation.X, l.MouseLocation.Y - 30, 100, 100).ToRectF(), selectedTextBrush);
                }
                else
                    rt.DrawLine(l.P1.ToPoint2F(), l.P2.ToPoint2F(), normalLineBrush, Model.LineWidth, strokeStyle);
            }
        }

        protected override IEnumerable<Line> GetObjects()
        {
            var center = ReferenceSystem.ScreenOriginalPoint;
            var angles = CalAngles(Model.ObjectNumber);
            var radius = Mapper.GetScreenX(ReferenceSystem.Right) - center.X;
            foreach (float angle in angles)
            {
                var x = center.X + radius * (float)Math.Sin(Functions.DegreeToRadian(angle));
                var y = center.Y - radius * (float)Math.Cos(Functions.DegreeToRadian(angle));
                yield return new Line(center, new PointF((float)x, (float)y)) { Value = angle };
            }
        }
    }
}
