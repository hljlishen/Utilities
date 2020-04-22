using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Drawing;
using System.Windows.Forms;
using Utilities.Tools;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.Display
{
    public class WaveGateSelector : GraphicElement, ISwtichable
    {
        private PointF corner1;
        private PointF corner2;
        private Brush fillBrush;

        private MouseDragDetector dragDetector;

        public string Name { get; set; } = "波门选择";

        public event Action<PointF, PointF> SelectionFinish;

        public override void Dispose()
        {
            base.Dispose();
            fillBrush?.Dispose();
        }

        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            fillBrush = Color.Yellow.SolidBrush(rt);
            fillBrush.Opacity = 0.8f;
        }

        protected override void BindEvents(Panel Panel)
        {
            dragDetector = new MouseDragDetector(Panel);
            dragDetector.MouseDrag += DragDetector_MouseDrag;
            dragDetector.MouseUp += DragDetector_MouseUp;
        }

        private void DragDetector_MouseUp(Point obj)
        {
            var dis = Functions.DistanceBetween(corner1.ToPoint2F(), corner2.ToPoint2F());
            if (dis < 10)
                return;
            SelectionFinish?.Invoke(corner1, corner2);
            corner2 = new PointF();
            corner1 = new PointF();
            UpdateView();
        }

        private void DragDetector_MouseDrag(Point arg1, Point arg2)
        {
            corner1 = arg1;
            corner2 = arg2;
            UpdateView();
        }

        protected override void UnbindEvents(Panel p)
        {
            dragDetector.MouseDrag -= DragDetector_MouseDrag;
            dragDetector.MouseUp -= DragDetector_MouseUp;
            dragDetector?.Dispose();
        }
        protected override void DrawElement(RenderTarget rt)
        {
            var geo = GetPathGeometry(rt, ReferenceSystem.ScreenOriginalPoint, corner1, corner2);
            rt.FillGeometry(geo, fillBrush);
        }

        public static PathGeometry GetPathGeometry(RenderTarget t, PointF OriginalPoint, PointF p1, PointF p2)
        {
            Point2F innerLeft, outterLeft, outterRight, innerRight;
            PathGeometry waveGate = t.Factory.CreatePathGeometry();

            double mouseBeginAngle = Functions.AngleToNorth(OriginalPoint, p1);
            double mouseEndAngle = Functions.AngleToNorth(OriginalPoint, p2);

            double begin = Functions.FindSmallArcBeginAngle(mouseBeginAngle, mouseEndAngle);
            double end = Functions.FindSmallArcEndAngle(mouseBeginAngle, mouseEndAngle);

            double mouseBeginDis = (float)Functions.DistanceBetween(OriginalPoint.ToPoint2F(), p1.ToPoint2F());
            double mouseEndDis = (float)Functions.DistanceBetween(OriginalPoint.ToPoint2F(), p2.ToPoint2F());
            Point2F mouseBeginZoomed = RadiusWiseZoomPosition(p1, mouseEndDis, OriginalPoint);
            Point2F mouseDragZoomed = RadiusWiseZoomPosition(p2, mouseBeginDis, OriginalPoint);

            if (begin == mouseBeginAngle)    //扇形在鼠标点击一侧开始顺时针扫过
            {
                if (mouseBeginDis < mouseEndDis) //鼠标向外拖
                {
                    innerLeft = p1.ToPoint2F();
                    outterLeft = mouseBeginZoomed;
                    outterRight = p2.ToPoint2F();
                    innerRight = mouseDragZoomed;
                }
                else    //鼠标向内拖
                {
                    innerLeft = mouseBeginZoomed;
                    outterLeft = p1.ToPoint2F();
                    outterRight = mouseDragZoomed;
                    innerRight = p2.ToPoint2F();
                }
            }
            else   //扇形在鼠标拖动一侧开始顺时针扫过
            {
                if (mouseBeginDis < mouseEndDis) //鼠标向外拖
                {
                    innerLeft = mouseDragZoomed;
                    outterLeft = p2.ToPoint2F();
                    outterRight = mouseBeginZoomed;
                    innerRight = p1.ToPoint2F();
                }
                else    //鼠标向内拖
                {
                    innerLeft = p2.ToPoint2F();
                    outterLeft = mouseDragZoomed;
                    outterRight = p1.ToPoint2F();
                    innerRight = mouseBeginZoomed;
                }
            }

            GeometrySink gs = waveGate.Open();
            gs.BeginFigure(innerLeft, FigureBegin.Filled);
            gs.AddLine(outterLeft);

            double disMax = Math.Max(mouseBeginDis, mouseEndDis);
            double disMin = Math.Min(mouseBeginDis, mouseEndDis);

            Microsoft.WindowsAPICodePack.DirectX.Direct2D1.SizeF size = new Microsoft.WindowsAPICodePack.DirectX.Direct2D1.SizeF((float)disMax, (float)disMax);
            ArcSegment arc = new ArcSegment(outterRight, size, 0, SweepDirection.Clockwise, ArcSize.Small);
            gs.AddArc(arc);

            gs.AddLine(innerRight);
            size = new Microsoft.WindowsAPICodePack.DirectX.Direct2D1.SizeF((float)disMin, (float)disMin);
            arc = new ArcSegment(innerLeft, size, 0, SweepDirection.Counterclockwise, ArcSize.Small);
            gs.AddArc(arc);
            gs.EndFigure(FigureEnd.Closed);
            gs.Close();
            gs.Dispose();

            return waveGate;
        }
        public static Point2F RadiusWiseZoomPosition(PointF p, double r, PointF o)
        {
            var ret = new Point2F();

            //计算拖拽位置和坐标原点连线的正北夹角
            var angle = Functions.AngleToNorth(o, p);
            angle = Functions.DegreeToRadian(angle);

            //计算起始角度对应直线与坐标系外圈圆周的交点坐标
            ret.X = (int)(o.X + r * Math.Sin(angle));
            ret.Y = (int)(o.Y - r * Math.Cos(angle));

            return ret;
        }   //极坐标

        public void On() => dragDetector.On();
        public void Off() => dragDetector.Off();
        public bool IsOn => dragDetector.IsOn;
    }
}
