using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Drawing;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.Display
{
    public class ZoomController : GraphicElement
    {
        private bool mouseDown = false;
        private Point mouseDownPos;
        private Point mouseCurrentPos;
        private Brush fillBrush;
        private Brush frameBrush;
        private Rectangle coverRect;
        public SelectStrategy SelectStrategy { get; set; }

        public override void Dispose()
        {
            base.Dispose();
            fillBrush?.Dispose();
            frameBrush?.Dispose();
        }

        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            fillBrush = Color.Blue.SolidBrush(rt);
            fillBrush.Opacity = 0.5f;
            frameBrush = Color.White.SolidBrush(rt);
        }

        public ZoomController() : this(new RectangleSelection()) { }
        public ZoomController(SelectStrategy selectStrategy)
        {
            SelectStrategy = selectStrategy;
            SelectStrategy.SetZoomController(this);
        }

        public override void SetDisplayer(Displayer d)
        {
            base.SetDisplayer(d);
            Panel.MouseDown += PictureBox_MouseDown;
            Panel.MouseMove += PictureBox_MouseMove;
            Panel.MouseUp += PictureBox_MouseUp;
        }

        private void PictureBox_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            mouseDown = false;
            if (!SelectStrategy.IsRectBigEnough(coverRect, Mapper))
            {
                Changed = true;
                return;
            }
            SetMapperRange(Mapper.GetCoordinateX(coverRect.Left), Mapper.GetCoordinateX(coverRect.Right), Mapper.GetCoordinateY(coverRect.Top), Mapper.GetCoordinateY(coverRect.Bottom));
        }

        private void SetMapperRange(double xLeft, double xRight, double yTop, double yBottom)
        {
            Mapper.SetCoordinateXRange(xLeft, xRight);
            Mapper.SetCoordinateYRange(yTop, yBottom);
            Changed = true;
        }

        private void PictureBox_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!mouseDown)
                return;
            var p = Panel;
            mouseCurrentPos = e.Location;
            coverRect = SelectStrategy.CalRect(mouseDownPos, mouseCurrentPos);
            Changed = true;
        }

        private void PictureBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            mouseDown = true;
            mouseDownPos = e.Location;
        }

        public void Reset() => SetMapperRange(ReferenceSystem.Left, ReferenceSystem.Right, ReferenceSystem.Top, ReferenceSystem.Bottom);

        protected override void DrawElement(RenderTarget rt)
        {
            if (!mouseDown)
                return;
            SelectStrategy.DrawZoomView(coverRect.ToRectF(), rt, fillBrush, frameBrush, 2);
        }

        public void SetStrategy(SelectStrategy s)
        {
            lock(Locker)
            {
                SelectStrategy = s;
                s.SetZoomController(this);
            }
        }
    }
}
