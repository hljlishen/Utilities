using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Coordinates;
using System.Drawing;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.Display
{
    public struct OriginVideoDotProperty
    {
        public PolarCoordinate Location;
        public int Am;
    }
    public class OriginalVideoDot : RotatableElement<OriginVideoDotProperty>
    {
        private Brush fillBrush;

        public override void Dispose()
        {
            base.Dispose();
            fillBrush?.Dispose();
        }

        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            fillBrush = Color.Red.SolidBrush(rt);   //需要动态计算画刷的颜色
        }
        public OriginalVideoDot(PolarCoordinate location, double am = 0, string rotateDecoratotInstanceName = "default") : base(rotateDecoratotInstanceName)
        {
            Model.Location = location;
        }

        public OriginalVideoDot(OriginVideoDotProperty p) : this(p.Location, p.Am) { }
        protected override void DrawDynamicElement(RenderTarget rt)
        {
            var rotatedPoint = new PolarCoordinate(Model.Location.Az + RotateAngle, Model.Location.El, Model.Location.Dis).Rectangular;
            var scrPoint = InnerMapper.GetScreenLocation(rotatedPoint.X, rotatedPoint.Y);
            Ellipse e = new Ellipse(scrPoint.ToPoint2F(), 3, 3);
            rt.FillEllipse(e, fillBrush);
        }
    }
}
