using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Drawing;
using System.Windows.Forms;

namespace Utilities.Display
{
    public class MouseDragger : GraphicElement, ISwtichable
    {
        double lastLeft;
        double lastRight;
        double lastTop;
        double lastBottom;
        MouseDragDetector dragDetector;

        public string Name { get; set; } = "拖动控制";

        protected override void DrawElement(RenderTarget rt)
        {
            //throw new NotImplementedException();
            //不需要绘制该元素
        }

        protected override void BindEvents(Panel p)
        {
            base.BindEvents(p);
            dragDetector = new MouseDragDetector(p);
            dragDetector.MouseDrag += DragDetector_MouseDrag;
            dragDetector.MouseDown += DragDetector_MouseDown;
        }

        private void DragDetector_MouseDown(Point obj) => SaveMapperState();

        private void SaveMapperState()
        {
            lastLeft = Mapper.CoordinateLeft;
            lastRight = Mapper.CoordinateRight;
            lastTop = Mapper.CoordinateTop;
            lastBottom = Mapper.CoordinateBottom;
        }

        private void DragDetector_MouseDrag(Point arg1, Point arg2)
        {
            var mouseDownCoo = Mapper.GetCoordinateLocation(arg1.X, arg1.Y);
            var mouseCurrentCoo = Mapper.GetCoordinateLocation(arg2.X, arg2.Y);
            var xDis = mouseCurrentCoo.X - mouseDownCoo.X;
            var yDis = mouseCurrentCoo.Y - mouseDownCoo.Y;

            Mapper.SetCoordinateArea(lastLeft - xDis, lastRight - xDis, lastTop - yDis, lastBottom - yDis);
        }

        protected override void UnbindEvents(Panel p)
        {
            base.UnbindEvents(p);
            dragDetector.MouseDrag -= DragDetector_MouseDrag;
            dragDetector.Dispose();
        }
        public void On() => dragDetector.On();
        public void Off() => dragDetector.Off();
        public bool IsOn => dragDetector.IsOn;
    }
}
