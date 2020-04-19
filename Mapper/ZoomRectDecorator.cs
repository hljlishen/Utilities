using System;
using System.Drawing;

namespace Utilities.Mapper
{
    public class ZoomRectDecorator : MapperDecorator
    {
        private double percentage;
        public ZoomRectDecorator(IScreenToCoordinateMapper mapper, double percentage) : base(mapper)
        {
            this.percentage = percentage;
        }
        public override void SetScreenArea(double left, double right, double top, double bottom) => SetScreenArea(ZoomRectangle(left, right, top, bottom, percentage));

        internal static Area ZoomRectangle(double left, double right, double top, double bottom, double percent)
        {
            var r = new Area();
            var zoomedWidth = Math.Abs(right - left) * percent;
            var zoomedHeight = Math.Abs(bottom - top) * percent;
            var center = new PointF((float)(right + left) / 2, (float)(bottom + top) / 2);

            r.Left = center.X - zoomedWidth / 2;
            r.Right = center.X + zoomedWidth / 2;
            r.Top = center.Y - zoomedHeight / 2;
            r.Bottom = center.Y + zoomedHeight / 2;

            return r;
        }
        internal void SetScreenArea(Area a)
        {
            Mapper.SetScreenArea(a.Left, a.Right, a.Top, a.Bottom);
            InvokeStateChanged();
        }
    }
}
