﻿using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Collections.Generic;
using System.Drawing;

namespace Utilities.Display
{
    public class XAxisMarker : MarkerElement
    {
        private StrokeStyle stroke;

        public override void Dispose()
        {
            base.Dispose();
            stroke?.Dispose();
        }

        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            stroke = rt.Factory.CreateStrokeStyle(new StrokeStyleProperties { DashStyle = DashStyle.Dash });
        }
        public XAxisMarker(MarkerModel model) : base(model)
        {
        }

        public XAxisMarker() : this(new MarkerModel(Color.Green, 1, 10, Color.Orange, 3, "宋体", 15, Color.Gray, "宋体", 15, Color.Orange)) { }

        protected override IEnumerable<LiveObject> GetObjects()
        {
            var step = (ReferenceSystem.Right - ReferenceSystem.Left) / Model.ObjectNumber;
            var yBottom = Mapper.GetScreenY(ReferenceSystem.Bottom);
            var yTop = Mapper.GetScreenY(ReferenceSystem.Top);
            for (int i = 0; i < Model.ObjectNumber + 1; i++)
            {
                double value = ReferenceSystem.Left + step * i;
                var x = Mapper.GetScreenX(value);
                yield return new LiveLine(new PointF((float)x, (float)yBottom), new PointF((float)x, (float)yTop)) { Value = value };
            }
        }

        protected void DrawObjectUnselected(RenderTarget rt, LiveLine l)
        {
            normalLineBrush.Opacity = 0.8f;
            rt.DrawLine(l.P1.ToPoint2F(), l.P2.ToPoint2F(), normalLineBrush, Model.LineWidth, stroke);
        }

        protected void DrawObjectSelected(RenderTarget rt, LiveLine l)
        {
            rt.DrawLine(l.P1.ToPoint2F(), l.P2.ToPoint2F(), selectedLineBrush, Model.SelectedLineWidth, stroke);
            rt.DrawText(l.Value.ToString(), selectedTextFormat, new RectangleF(l.MouseLocation.X + 20, l.MouseLocation.Y, 100, 100).ToRectF(), selectedTextBrush);
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
