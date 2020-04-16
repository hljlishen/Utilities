using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Drawing;
using Utilities.Coordinates;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.Display
{
    public class NorthLine : RotatableElement<double>
    {
        private Brush LineBrush;

        public NorthLine(string rotateDecoratotInstanceName="default") :base(rotateDecoratotInstanceName)
        {
        }

        public override void Dispose()
        {
            base.Dispose();
            LineBrush.Dispose();
        }

        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            LineBrush = Color.Red.SolidBrush(rt);
            Model = 30;
        }
        protected override void DrawDynamicElement(RenderTarget rt)
        {
            PolarCoordinate p = new PolarCoordinate(Model, 0, ReferenceSystem.Top);
            var r = p.Rectangular;
            var scrP = Mapper.GetScreenLocation(r.X, r.Y);
            rt.DrawLine(scrP.ToPoint2F(), ReferenceSystem.ScreenOriginalPoint.ToPoint2F(), LineBrush, 3);
        }
    }
}
