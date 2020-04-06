using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Drawing;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.Display
{
    public class AxisX : GraphicElement
    {
        private Brush axisBrush;
        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            axisBrush = Color.White.SolidBrush(rt);
        }

        public override void Dispose()
        {
            base.Dispose();
            axisBrush?.Dispose();
        }
        protected override void DrawElement(RenderTarget rt)
        {
            var screenLeft = Mapper.GetScreenX(ReferenceSystem.Left);
            var screenRight = Mapper.GetScreenX(ReferenceSystem.Right);
            var originalPoint = ReferenceSystem.ScreenOriginalPoint;

            rt.DrawLine(new Point2F((float)screenLeft, originalPoint.Y), new Point2F((float)screenRight, originalPoint.Y), axisBrush, 2);
        }
    }
}
