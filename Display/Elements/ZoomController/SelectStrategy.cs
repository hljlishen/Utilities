using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Drawing;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;
using Utilities.Mapper;

namespace Utilities.Display
{
    public abstract class SelectStrategy
    {
        protected ZoomController ctrl;
        public IScreenToCoordinateMapper Mapper => ctrl.Mapper;
        public ReferenceSystem ReferencSystem => ctrl.ReferenceSystem;

        public void SetZoomController(ZoomController ctrl) => this.ctrl = ctrl ?? throw new ArgumentNullException(nameof(ctrl));
        public abstract Rectangle CalRect(Point centerPoint, Point CornerPoint);
        public virtual void DrawZoomView(RectF coverRect, RenderTarget rt, Brush fillBrush, Brush frameBrush, float strokeWidth)
        {
            rt.FillRectangle(coverRect, fillBrush);
            rt.DrawRectangle(coverRect, frameBrush, strokeWidth);
        }

        public virtual bool IsRectBigEnough(Rectangle r, IScreenToCoordinateMapper mapper)
        {
            if (Math.Abs(r.Right - r.Left) < 20 || Math.Abs(r.Bottom - r.Top) < 20)
                return false;
            var xLeft = mapper.GetCoordinateX(r.Left);
            var xRight = mapper.GetCoordinateX(r.Right);
            var yTop = mapper.GetCoordinateY(r.Top);
            var yBottom = mapper.GetCoordinateY(r.Bottom);
            if(ValueMapper.IsIntervalTooSmall(xLeft, xRight) || ValueMapper.IsIntervalTooSmall(yTop, yBottom))
                return false;
            return true;
        }
    }
}
