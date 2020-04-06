using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Collections.Generic;
using System.Drawing;

namespace Utilities.Display
{
    public class YAxisMarker : MouseMoveElement<Line>
    {
        private StrokeStyle stroke;

        public YAxisMarker() : this(new MarkerModel(Color.Green, 1, 10, Color.Orange, 3, "宋体", 15, Color.Gray, "宋体", 15, Color.Orange)) { }
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

        protected override IEnumerable<Line> GetObjects()
        {
            var step = (ReferenceSystem.Top - ReferenceSystem.Bottom) / Model.ObjectNumber;
            var xLeft = Mapper.GetScreenX(ReferenceSystem.Left);
            var xRight = Mapper.GetScreenX(ReferenceSystem.Right);

            for (int i = 0; i < Model.ObjectNumber; i++)
            {
                double value = ReferenceSystem.Bottom + step * i;
                var y = Mapper.GetScreenY(value);
                yield return new Line(new PointF((float)xLeft, (float)y), new PointF((float)xRight, (float)y)) { Value = value };
            }
        }

        protected override void DrawDynamicElement(RenderTarget rt)
        {
            foreach (var o in objects)
            {
                var l = o as Line;
                if (l.Selected)
                {
                    rt.DrawLine(l.P1.ToPoint2F(), l.P2.ToPoint2F(), selectedLineBrush, Model.SelectedLineWidth, stroke);
                    rt.DrawText(l.Value.ToString(), selectedTextFormat, new RectangleF(l.MouseLocation.X, l.MouseLocation.Y - 30, 100, 100).ToRectF(), selectedTextBrush);
                }
                else
                {
                    rt.DrawLine(l.P1.ToPoint2F(), l.P2.ToPoint2F(), normalLineBrush, Model.LineWidth, stroke);
                }
            }
        }

        public YAxisMarker(MarkerModel model) : base(model)
        {
        }
    }
}
