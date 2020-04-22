//using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
//using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
//using System;
//using System.Drawing;
//using System.Windows.Forms;
//using Utilities.Coordinates;
//using Utilities.Mapper;
//using Utilities.Tools;
//using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

//namespace Utilities.Display
//{
//    public class RotationController : RotatableElement<double>, ISwtichable
//    {
//        private bool MouseDown = false;
//        private PointF MouseDownPos;
//        private PointF MouseCurrentPos;
//        private Brush markerBrush;
//        private Brush selectedMarkerBrush;
//        private TextFormat textFormat;
//        private TextLayout TextLayout;
//        private Brush textBrush;
//        private double lastAngle;
//        private bool isOn = false;

//        public RotationController(string rotateDecoratotInstanceName = "default") : base(rotateDecoratotInstanceName)
//        {
//        }

//        public bool IsOn => isOn;

//        public string Name { get; set; } = "旋转控制";

//        protected override void InitializeComponents(RenderTarget rt)
//        {
//            base.InitializeComponents(rt);
//            markerBrush = Color.Gray.SolidBrush(rt);
//            selectedMarkerBrush = Color.Yellow.SolidBrush(rt);
//            textBrush = Color.White.SolidBrush(rt);
//            string fontName = "微软雅黑";
//            textFormat = fontName.MakeFormat(10);
//        }

//        public override void Dispose()
//        {
//            base.Dispose();
//            markerBrush.Dispose();
//            selectedMarkerBrush.Dispose();
//            textBrush.Dispose();
//        }
//        protected override void DrawDynamicElement(RenderTarget rt)
//        {
//            var r = Math.Abs(ReferenceSystem.Right);
//            var r1 = r * 0.925;
//            var r2 = r * 0.95;
//            var r3 = r * 1.05;
//            for(int  i = 0; i < 360; i++)
//            {
//                var p1 = new PolarCoordinate(i, 0, r);
//                PolarCoordinate p2;
//                if (i % 5 == 0)
//                    p2 = new PolarCoordinate(i, 0, r1);
//                else
//                    p2 = new PolarCoordinate(i, 0, r2);
//                var p3 = new PolarCoordinate(i, 0, r3);

//                var scrP1 = Mapper.GetScreenLocation(p1.X, p1.Y);
//                var scrP2 = Mapper.GetScreenLocation(p2.X, p2.Y);
//                var scrP3 = Mapper.GetScreenLocation(p3.X, p3.Y);
//                if (!MouseDown)
//                    rt.DrawLine(scrP1.ToPoint2F(), scrP2.ToPoint2F(), markerBrush, 1);
//                else
//                {
//                    rt.DrawLine(scrP1.ToPoint2F(), scrP2.ToPoint2F(), selectedMarkerBrush, 1);
//                }

//                if(i % 5 == 0)
//                    rt.DrawText(i.ToString(), textFormat, new RectangleF(scrP3.X - 5, scrP3.Y - 5, 100, 100).ToRectF(), textBrush);
//            }
//        }

//        protected override void BindEvents(Panel p)
//        {
//            base.BindEvents(p);
//            Panel.MouseDown += Panel_MouseDown;
//            Panel.MouseMove += Panel_MouseMove;
//            Panel.MouseUp += Panel_MouseUp;
//        }

//        protected override void UnbindEvents(Panel p)
//        {
//            base.UnbindEvents(p);
//            Panel.MouseDown -= Panel_MouseDown;
//            Panel.MouseMove -= Panel_MouseMove;
//            Panel.MouseUp -= Panel_MouseUp;
//        }

//        private void Panel_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
//        {
//            MouseDown = false;
//            lastAngle = (Mapper as PolarRotateDecorator).RotateAngle;
//            UpdateView();
//        }

//        private void Panel_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
//        {
//            if (!MouseDown)
//                return;
//            MouseCurrentPos = e.Location;
//            var angle1 = Functions.AngleToNorth(ReferenceSystem.ScreenOriginalPoint, MouseDownPos);
//            var angle2 = Functions.AngleToNorth(ReferenceSystem.ScreenOriginalPoint, MouseCurrentPos);
//            var diff = (angle2 - angle1);
//            (Mapper as PolarRotateDecorator).RotateAngle = lastAngle + diff;
//            UpdateView();
//        }

//        private void Panel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
//        {
//            if (!IsOn)
//                return;
//            MouseDown = true;
//            MouseDownPos = e.Location;
//            MouseCurrentPos = e.Location;
//        }

//        public void On() => isOn = true;

//        public void Off() => isOn = false;
//    }
//}
