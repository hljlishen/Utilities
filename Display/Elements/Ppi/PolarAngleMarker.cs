using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Drawing;
using Utilities.Tools;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.Display
{
    public class PolarAngleMarker : RotatableElement<MarkerModel>
    {
        private StrokeStyle strokeStyle;
        protected Brush normalLineBrush;
        protected Brush selectedLineBrush;
        protected TextFormat normalTextFormat;
        protected TextFormat selectedTextFormat;
        protected Brush normalTextBrush;
        protected Brush selectedTextBrush;

        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            normalLineBrush = Model.LineColor.SolidBrush(rt);
            selectedLineBrush = Model.SelectedLineColor.SolidBrush(rt);
            normalTextBrush = Model.FontColor.SolidBrush(rt);
            selectedTextBrush = Model.SelectedFontColor.SolidBrush(rt);
            using (var dw = DWriteFactory.CreateFactory())
            {
                normalTextFormat = dw.CreateTextFormat(Model.FontName, Model.FontSize);
                selectedTextFormat = dw.CreateTextFormat(Model.SelectedFontName, Model.SelectedFontSize);
            }
            strokeStyle = rt.Factory.CreateStrokeStyle(new StrokeStyleProperties { DashStyle = DashStyle.DashDot });
        }

        public override void Dispose()
        {
            base.Dispose();
            normalLineBrush?.Dispose();
            selectedLineBrush?.Dispose();
            normalTextFormat?.Dispose();
            selectedTextFormat?.Dispose();
            normalTextBrush?.Dispose();
            selectedTextBrush?.Dispose();
            strokeStyle?.Dispose();
        }

        public PolarAngleMarker(MarkerModel model, string rotateDecoratorName = "default") : base(rotateDecoratorName)
        {
            Model = model;
        }

        public PolarAngleMarker(string rotateDecoratorName = "default") : this(new MarkerModel() { LineColor = Color.White, LineWidth = 1, ObjectNumber = 12, SelectedLineColor = Color.Orange, SelectedLineWidth = 3, FontName = "Berlin Sans FB Demi", FontColor = Color.Gray, FontSize = 15, SelectedFontColor = Color.Yellow, SelectedFontName = "Berlin Sans FB Demi", SelectedFontSize = 15 }, rotateDecoratorName) { }

        private IEnumerable<double> CalAngles(uint angleMarkerNumber)
        {
            double step = (double)360 / angleMarkerNumber;
            for (int i = 0; i < angleMarkerNumber; i++)
            {
                yield return step * i;
            }
        }

        protected override IEnumerable<LiveObject> GetObjects()
        {
            var center = ReferenceSystem.ScreenOriginalPoint;
            var angles = CalAngles(Model.ObjectNumber);
            var radius = InnerMapper.GetScreenLocation(ReferenceSystem.Right, 0).X - center.X;
            foreach (float angle in angles)
            {
                var x = center.X + radius * (float)Math.Sin(Functions.DegreeToRadian(angle + RotateAngle));
                var y = center.Y - radius * (float)Math.Cos(Functions.DegreeToRadian(angle + RotateAngle));
                yield return new LiveLine(center, new PointF(x, y)) { Value = angle };
            }
        }

        protected void DrawObjectUnselected(RenderTarget rt, LiveLine l)
        {
            rt.DrawLine(l.P1.ToPoint2F(), l.P2.ToPoint2F(), normalLineBrush, Model.LineWidth, strokeStyle);
        }

        protected void DrawObjectSelected(RenderTarget rt, LiveLine l)
        {
            rt.DrawLine(l.P1.ToPoint2F(), l.P2.ToPoint2F(), selectedLineBrush, Model.SelectedLineWidth, strokeStyle);
            rt.DrawText(l.Value.ToString() + "°", selectedTextFormat, new RectangleF(l.MouseLocation.X, l.MouseLocation.Y - 30, 100, 100).ToRectF(), selectedTextBrush);
        }

        protected override void DrawDynamicElement(RenderTarget rt)
        {
            foreach (var o in Objects)
            {
                var l = o as LiveLine;
                if (l.Selected)
                    DrawObjectSelected(rt, l);
                else
                    DrawObjectUnselected(rt, l);
            }
        }
    }
}
