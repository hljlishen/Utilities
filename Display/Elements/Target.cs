using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Coordinates;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.Display
{
    public class Target : RotatableElement<PolarCoordinate>
    {
        private Brush TargetBrush;
        private Brush TagBrush;
        private Brush TextBrush;
        private TextFormat TextFormat;
        public double Az { get; set; } = 90;
        public double Dis { get; set; } = 500;

        public override void Dispose()
        {
            base.Dispose();
            TargetBrush.Dispose();
            TagBrush.Dispose();
            TextBrush.Dispose();
            TextFormat.Dispose();
        }

        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            TargetBrush = Color.Red.SolidBrush(rt);
            TagBrush = Color.Red.SolidBrush(rt);
            TextBrush = Color.White.SolidBrush(rt);
            var fontName = "微软雅黑";
            TextFormat = fontName.MakeFormat(10);
        }
        protected override void DrawDynamicElement(RenderTarget rt)
        {
            var r = new PolarCoordinate(Az, 0, Dis).Rectangular;
            var scrP = Mapper.GetScreenLocation(r.X, r.Y);
            Ellipse e = new Ellipse(scrP.ToPoint2F(), 5, 5);
            rt.FillEllipse(e, TargetBrush);

            RectangleF rect = new RectangleF(scrP.X - 20, scrP.Y - 20, 40, 15);
            rt.FillRectangle(rect.ToRectF(), TagBrush);
            rt.DrawText("1", TextFormat, rect.ToRectF(), TextBrush);
        }
    }
}
