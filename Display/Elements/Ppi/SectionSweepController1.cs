using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Drawing;
using System.Windows.Forms;
using Utilities.Tools;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;
using SizeF = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.SizeF;

namespace Utilities.Display
{
    public enum ArcDirection
    { 
        ClockWise,
        COunterClockWise
    }

    public struct SweepSection
    {
        public double Begin;
        public double End;
        public ArcDirection ArcDirection;

        public SweepSection(double begin, double end) : this()
        {
            Begin = begin;
            End = end;
        }
    }
    public abstract class SectionSweepController : RotatableElement<SweepSection>, ISwtichable
    {
        public event Action<SectionSweepController, SweepSection> SectionSelected;
        private Brush frameBrush;
        private Brush fillBrush;
        private bool isActive = true;
        private bool isStopped = false;
        private MouseDragDetector dragDetector;

        protected SectionSweepController(string rotateDecoratotInstanceName = "default") : base(rotateDecoratotInstanceName)
        {
        }

        public string Name { get; set; } = "扇扫选择";

        public void Stop()
        {
            isStopped = true;
            UpdateView();
        }

        public void Start()
        {
            isStopped = false;
            UpdateView();
        }

        public override void Dispose()
        {
            base.Dispose();
            frameBrush?.Dispose();
            fillBrush?.Dispose();
        }

        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            frameBrush = Color.White.SolidBrush(rt);
            frameBrush.Opacity = 1f;
            fillBrush = Color.Red.SolidBrush(rt);
            fillBrush.Opacity = 0.3f;
        }

        protected override void BindEvents(Panel p)
        {
            dragDetector = new MouseDragDetector(p);
            dragDetector.MouseDrag += DragDetector_MouseDrag;
            dragDetector.MouseUp += DragDetector_MouseUp;
        }

        private void DragDetector_MouseUp(Point obj)
        {
            if (isActive && Math.Abs(Model.Begin - Model.End) > 15)
            {
                SectionSelected?.Invoke(this, Model);
            }
        }

        private void DragDetector_MouseDrag(Point arg1, Point arg2)
        {
            lock (Locker)
            {
                if (isActive && Functions.DistanceBetween(arg1, arg2) > 20)
                {
                    Model = GetSection(arg1, arg2);
                    UpdateView();
                }
            }
        }

        protected override void UnbindEvents(Panel p)
        {
            dragDetector.MouseDrag -= DragDetector_MouseDrag;
            dragDetector.MouseUp -= DragDetector_MouseUp;
            dragDetector.Dispose();
        }
        protected abstract PathGeometry GetPathGeometry(SweepSection s, RenderTarget t);
        protected abstract SweepSection GetSection(PointF downPos, PointF currentPos);
        protected override void DrawDynamicElement(RenderTarget rt)
        {
            if (isActive && !isStopped)
            {
                if (Model.Begin == 0 && Model.End == 0)
                    return;
                var geo = GetPathGeometry(Model, rt);
                rt.DrawGeometry(geo, frameBrush, 3);
                rt.FillGeometry(geo, fillBrush);
                geo.Dispose();
            }
        }
        public void On() => dragDetector.On();
        public void Off() => dragDetector.Off();
        public bool IsOn => dragDetector.IsOn;
    }

    public class PolarSectionSweepController : SectionSweepController
    {
        protected override PathGeometry GetPathGeometry(SweepSection s, RenderTarget t)
        {
            var pbegin = CalIntersectionPoint(s.Begin + RotateAngle);
            var pend = CalIntersectionPoint(s.End + RotateAngle);

            PathGeometry sweepSectionGraphic = t.Factory.CreatePathGeometry();
            GeometrySink gs = sweepSectionGraphic.Open();
            Point2F oPoint = ReferenceSystem.ScreenOriginalPoint.ToPoint2F();
            gs.BeginFigure(oPoint, FigureBegin.Filled);
            gs.AddLine(pbegin.ToPoint2F());
            //扇形的X轴Y轴半径是矩形框width的一半
            SizeF size = new SizeF((float)ReferenceSystem.ScreenWidth/ 2, (float)ReferenceSystem.ScreenWidth / 2);

            //添加弧线
            ArcSegment arc = new ArcSegment(pend.ToPoint2F(), size, 0, SweepDirection.Clockwise, ArcSize.Small);
            gs.AddArc(arc);

            //添加第二条线
            gs.AddLine(oPoint);
            gs.EndFigure(FigureEnd.Closed);
            gs.Close();
            gs.Dispose();

            return sweepSectionGraphic;
        }

        protected override SweepSection GetSection(PointF downPos, PointF currentPos)
        {
            var oPoint = ReferenceSystem.ScreenOriginalPoint;
            double angle1 = 90 - Functions.RadianToDegree(Math.Atan2(oPoint.Y - downPos.Y, downPos.X - oPoint.X));
            double angle2 = 90 - Functions.RadianToDegree(Math.Atan2(oPoint.Y - currentPos.Y, currentPos.X - oPoint.X));

            double maxA = Math.Max(angle1, angle2);
            double minA = Math.Min(angle1, angle2);
            double cover1 = maxA - minA;
            double cover2 = (360 - maxA) + minA;

            if (cover1 < cover2)
                return new SweepSection(minA - RotateAngle, maxA - RotateAngle);
            else
                return new SweepSection(maxA - RotateAngle, minA - RotateAngle);
        }
        private PointF CalIntersectionPoint(double angle)
        {
            double x = Math.Abs(ReferenceSystem.ScreenWidth) * Math.Sin(Functions.DegreeToRadian(angle)) / 2 + ReferenceSystem.ScreenOriginalPoint.X;
            double y = ReferenceSystem.ScreenOriginalPoint.Y - ReferenceSystem.ScreenWidth * Math.Cos(Functions.DegreeToRadian(angle)) / 2;

            return new PointF((float)x, (float)y);
        }
    }
}
