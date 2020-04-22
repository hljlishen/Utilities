using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Drawing;
using System.Windows.Forms;
using Utilities.Mapper;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.Display
{
    public class ZoomController : GraphicElement, ISwtichable
    {
        private Brush fillBrush;
        private Brush frameBrush;
        private Rectangle coverRect;
        private MouseDragDetector dragDetector;
        public bool Animation { get; set; } = true;
        public SelectStrategy SelectStrategy { get; protected set; }

        public string Name { get; set; } = "放缩控制";

        public override void Dispose()
        {
            base.Dispose();
            fillBrush?.Dispose();
            frameBrush?.Dispose();
        }

        protected override void UnbindEvents(Panel p)
        {
            dragDetector.MouseDrag -= DragDetector_MouseDrag;
            dragDetector.MouseUp -= DragDetector_MouseUp;
            dragDetector.Dispose();
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

        protected override void BindEvents(Panel p)
        {
            dragDetector = new MouseDragDetector(p);
            dragDetector.MouseDrag += DragDetector_MouseDrag;
            dragDetector.MouseUp += DragDetector_MouseUp;
        }

        private void DragDetector_MouseUp(Point obj)
        {
            if (!SelectStrategy.IsRectBigEnough(coverRect, Mapper))
            {
                UpdateView();
            }
            else
            {
                var left = Mapper.GetCoordinateX(coverRect.Left);
                var right = Mapper.GetCoordinateX(coverRect.Right);
                var top = Mapper.GetCoordinateY(coverRect.Top);
                var bottom = Mapper.GetCoordinateY(coverRect.Bottom);
                SetMapperRange(left, right, top, bottom);
            }

            //此处必须清零coverRect，原因是：当鼠标只做点击不拖动时，coverRect会保留上次放缩时计算的CoverRect值，因此会通过IsRectBigEnough的校验
            coverRect = new Rectangle(0, 0, 0, 0);
            return;
        }

        private void DragDetector_MouseDrag(Point arg1, Point arg2)
        {
            coverRect = SelectStrategy.CalRect(arg1, arg2);
            UpdateView();
        }

        private void SetMapperRange(double left, double right, double top, double bottom)
        {
            if (!Animation)
                Mapper.SetCoordinateArea(left, right, top, bottom);
            else
            {
                Area targetArea = new Area(left, right, top, bottom);
                ZoomAnimation zoomAnimation = new ZoomAnimation(targetArea, Mapper);
                zoomAnimation.StartZoom();
            }
        }

        public void Reset() => SetMapperRange(ReferenceSystem.Left, ReferenceSystem.Right, ReferenceSystem.Top, ReferenceSystem.Bottom);

        protected override void DrawElement(RenderTarget rt)
        {
            SelectStrategy.DrawZoomView(coverRect.ToRectF(), rt, fillBrush, frameBrush, 2);
        }

        public void SetStrategy(SelectStrategy s)
        {
            lock (Locker)
            {
                SelectStrategy = s;
                s.SetZoomController(this);
            }
        }

        public void On() => dragDetector.On();
        public void Off() => dragDetector.Off();
        public bool IsOn => dragDetector.IsOn;
    }
}
