//using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
//using System;
//using System.Drawing;
//using System.Windows.Forms;

//namespace Utilities.Display
//{
//    public class MouseDragger : GraphicElement, ISwtichable
//    {

//        protected bool mouseDown = false;
//        Point mouseDownPos;
//        Point mouseCurrentPos;
//        bool isOn = false;
//        double lastLeft;
//        double lastRight;
//        double lastTop;
//        double lastBottom;

//        public bool IsOn => isOn;

//        public string Name { get; set; } = "拖动控制";

//        protected override void DrawElement(RenderTarget rt)
//        {
//            //throw new NotImplementedException();
//            //不需要绘制该元素
//        }

//        protected override void BindEvents(Panel p)
//        {
//            base.BindEvents(p);
//            p.MouseDown += P_MouseDown;
//            p.MouseMove += P_MouseMove; 
//            p.MouseUp += P_MouseUp;
//        }

//        protected override void UnbindEvents(Panel p)
//        {
//            base.UnbindEvents(p);
//            p.MouseDown -= P_MouseDown;
//            p.MouseMove -= P_MouseMove;
//            p.MouseUp -= P_MouseUp;
//        }

//        private void P_MouseUp(object sender, MouseEventArgs e)
//        {
//            mouseDown = false;
//        }

//        private void P_MouseMove(object sender, MouseEventArgs e)
//        {
//            if (!mouseDown)
//                return;
//            mouseCurrentPos = e.Location;
//            var mouseDownCoo = Mapper.GetCoordinateLocation(mouseDownPos.X, mouseDownPos.Y);
//            var mouseCurrentCoo = Mapper.GetCoordinateLocation(mouseCurrentPos.X, mouseCurrentPos.Y);
//            var xDis = mouseCurrentCoo.X - mouseDownCoo.X;
//            Console.WriteLine(xDis);
//            var yDis = mouseCurrentCoo.Y - mouseDownCoo.Y;

//            Mapper.SetCoordinateArea(lastLeft - xDis, lastRight - xDis, lastTop - yDis, lastBottom - yDis);
//        }

//        private void P_MouseDown(object sender, MouseEventArgs e)
//        {
//            if (!isOn)
//                return;
//            mouseDown = true;
//            mouseDownPos = e.Location;
//            mouseCurrentPos = e.Location;
//            lastLeft = Mapper.CoordinateLeft;
//            lastRight = Mapper.CoordinateRight;
//            lastTop = Mapper.CoordinateTop;
//            lastBottom = Mapper.CoordinateBottom;
//        }

//        public void On()
//        {
//            //if (isOn)
//            //    return;
//            //BindEvents(Panel);
//            isOn = true;
//        }

//        public void Off()
//        {
//            //if (!isOn)
//            //    return;
//            //UnbindEvents(Panel);
//            isOn = false;
//        }
//    }
//}
