//using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
//using System;
//using System.Drawing;
//using System.Windows.Forms;
//using Utilities.Tools;
//using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

//namespace Utilities.Display
//{
//    public class WaveGateSelector : GraphicElement, ISwtichable
//    {
//        private bool MouseDown = false;
//        private PointF MouseDownPos;
//        private PointF MouseCurrentPos;
//        private Brush fillBrush;
//        private bool isOn = false;

//        public bool IsOn => isOn;

//        public string Name { get; set; } = "波门选择";

//        public event Action<PointF, PointF> SelectionFinish;

//        public override void Dispose()
//        {
//            base.Dispose();
//            fillBrush?.Dispose();
//        }

//        protected override void InitializeComponents(RenderTarget rt)
//        {
//            base.InitializeComponents(rt);
//            fillBrush = Color.Yellow.SolidBrush(rt);
//            fillBrush.Opacity = 0.8f;
//        }

//        protected override void BindEvents(Panel Panel)
//        {
//            Panel.MouseDown += Panel_MouseDown;
//            Panel.MouseMove += Panel_MouseMove;
//            Panel.MouseUp += Panel_MouseUp;
//        }

//        protected override void UnbindEvents(Panel p)
//        {
//            Panel.MouseDown -= Panel_MouseDown;
//            Panel.MouseMove -= Panel_MouseMove;
//            Panel.MouseUp -= Panel_MouseUp;
//        }

//        private void Panel_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
//        {
//            if (!MouseDown)
//                return;
//            MouseDown = false;
//            var dis = Functions.DistanceBetween(MouseDownPos.ToPoint2F(), MouseCurrentPos.ToPoint2F());
//            if (dis < 10)
//                return;
//            SelectionFinish?.Invoke(MouseDownPos, MouseCurrentPos);
//            UpdateView();
//        }

//        private void Panel_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
//        {
//            lock (Locker)
//            {
//                if (!MouseDown)
//                    return;
//                MouseCurrentPos = e.Location;
//                UpdateView();
//            }
//        }

//        private void Panel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
//        {
//            lock(Locker)
//            {
//                if (!IsOn)
//                    return;
//                MouseDown = true;
//                MouseDownPos = e.Location;
//                MouseCurrentPos = e.Location;
//            }
//        }

//        protected override void DrawElement(RenderTarget rt)
//        {
//            if (!MouseDown)
//                return;
//            var geo = GetPathGeometry(rt, ReferenceSystem.ScreenOriginalPoint, MouseDownPos, MouseCurrentPos);
//            rt.FillGeometry(geo, fillBrush);
//        }

//        public static PathGeometry GetPathGeometry(RenderTarget t, PointF OriginalPoint, PointF p1, PointF p2)
//        {
//            Point2F innerLeft, outterLeft, outterRight, innerRight;
//            PathGeometry waveGate = t.Factory.CreatePathGeometry();

//            double mouseBeginAngle = Functions.AngleToNorth(OriginalPoint, p1);
//            double mouseEndAngle = Functions.AngleToNorth(OriginalPoint, p2);

//            double begin = Functions.FindSmallArcBeginAngle(mouseBeginAngle, mouseEndAngle);
//            double end = Functions.FindSmallArcEndAngle(mouseBeginAngle, mouseEndAngle);

//            double mouseBeginDis = (float)Functions.DistanceBetween(OriginalPoint.ToPoint2F(), p1.ToPoint2F());
//            double mouseEndDis = (float)Functions.DistanceBetween(OriginalPoint.ToPoint2F(), p2.ToPoint2F());
//            Point2F mouseBeginZoomed = RadiusWiseZoomPosition(p1, mouseEndDis, OriginalPoint);
//            Point2F mouseDragZoomed = RadiusWiseZoomPosition(p2, mouseBeginDis, OriginalPoint);

//            if (begin == mouseBeginAngle)    //扇形在鼠标点击一侧开始顺时针扫过
//            {
//                if (mouseBeginDis < mouseEndDis) //鼠标向外拖
//                {
//                    innerLeft = p1.ToPoint2F();
//                    outterLeft = mouseBeginZoomed;
//                    outterRight = p2.ToPoint2F();
//                    innerRight = mouseDragZoomed;
//                }
//                else    //鼠标向内拖
//                {
//                    innerLeft = mouseBeginZoomed;
//                    outterLeft = p1.ToPoint2F();
//                    outterRight = mouseDragZoomed;
//                    innerRight = p2.ToPoint2F();
//                }
//            }
//            else   //扇形在鼠标拖动一侧开始顺时针扫过
//            {
//                if (mouseBeginDis < mouseEndDis) //鼠标向外拖
//                {
//                    innerLeft = mouseDragZoomed;
//                    outterLeft = p2.ToPoint2F();
//                    outterRight = mouseBeginZoomed;
//                    innerRight = p1.ToPoint2F();
//                }
//                else    //鼠标向内拖
//                {
//                    innerLeft = p2.ToPoint2F();
//                    outterLeft = mouseDragZoomed;
//                    outterRight = p1.ToPoint2F();
//                    innerRight = mouseBeginZoomed;
//                }
//            }

//            GeometrySink gs = waveGate.Open();
//            gs.BeginFigure(innerLeft, FigureBegin.Filled);
//            gs.AddLine(outterLeft);

//            double disMax = Math.Max(mouseBeginDis, mouseEndDis);
//            double disMin = Math.Min(mouseBeginDis, mouseEndDis);

//            Microsoft.WindowsAPICodePack.DirectX.Direct2D1.SizeF size = new Microsoft.WindowsAPICodePack.DirectX.Direct2D1.SizeF((float)disMax, (float)disMax);
//            ArcSegment arc = new ArcSegment(outterRight, size, 0, SweepDirection.Clockwise, ArcSize.Small);
//            gs.AddArc(arc);

//            gs.AddLine(innerRight);
//            size = new Microsoft.WindowsAPICodePack.DirectX.Direct2D1.SizeF((float)disMin, (float)disMin);
//            arc = new ArcSegment(innerLeft, size, 0, SweepDirection.Counterclockwise, ArcSize.Small);
//            gs.AddArc(arc);
//            gs.EndFigure(FigureEnd.Closed);
//            gs.Close();
//            gs.Dispose();

//            return waveGate;
//        }
//        public static Point2F RadiusWiseZoomPosition(PointF p, double r, PointF o)
//        {
//            var ret = new Point2F();

//            //计算拖拽位置和坐标原点连线的正北夹角
//            var angle = Functions.AngleToNorth(o, p);
//            angle = Functions.DegreeToRadian(angle);

//            //计算起始角度对应直线与坐标系外圈圆周的交点坐标
//            ret.X = (int)(o.X + r * Math.Sin(angle));
//            ret.Y = (int)(o.Y - r * Math.Cos(angle));

//            return ret;
//        }   //极坐标

//        public void On() => isOn = true;

//        public void Off() => isOn = false;
//    }
//}
