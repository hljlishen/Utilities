//using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
//using System.Drawing;
//using System.Windows.Forms;
//using Utilities.Mapper;
//using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

//namespace Utilities.Display
//{
//    public class ZoomController : GraphicElement, ISwtichable
//    {
//        private bool mouseDown = false;
//        private Point mouseDownPos;
//        private Point mouseCurrentPos;
//        private Brush fillBrush;
//        private Brush frameBrush;
//        private Rectangle coverRect;
//        private bool isOn = false;
//        public bool Animation { get; set; } = true;
//        public SelectStrategy SelectStrategy { get; protected set; }

//        public bool IsOn => isOn;

//        public string Name { get; set; } = "放缩控制";

//        public override void Dispose()
//        {
//            base.Dispose();
//            fillBrush?.Dispose();
//            frameBrush?.Dispose();
//        }

//        protected override void UnbindEvents(Panel p)
//        {
//            p.MouseDown -= PictureBox_MouseDown;
//            p.MouseMove -= PictureBox_MouseMove;
//            p.MouseUp -= PictureBox_MouseUp;
//            p.MouseDoubleClick -= P_MouseDoubleClick;
//        }

//        protected override void InitializeComponents(RenderTarget rt)
//        {
//            base.InitializeComponents(rt);
//            fillBrush = Color.Blue.SolidBrush(rt);
//            fillBrush.Opacity = 0.5f;
//            frameBrush = Color.White.SolidBrush(rt);
//        }

//        public ZoomController() : this(new RectangleSelection()) { }
//        public ZoomController(SelectStrategy selectStrategy)
//        {
//            SelectStrategy = selectStrategy;
//            SelectStrategy.SetZoomController(this);
//        }

//        protected override void BindEvents(Panel p)
//        {
//            p.MouseDown += PictureBox_MouseDown;
//            p.MouseMove += PictureBox_MouseMove;
//            p.MouseUp += PictureBox_MouseUp;
//            p.MouseDoubleClick += P_MouseDoubleClick;
//        }

//        private void P_MouseDoubleClick(object sender, MouseEventArgs e)
//        {
//            mouseDown = false;
//        }

//        private void PictureBox_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
//        {
//            if (!mouseDown)
//                return;
//            mouseDown = false;

//            if (!SelectStrategy.IsRectBigEnough(coverRect, Mapper))
//            {
//                UpdateView();
//            }
//            else
//            {
//                var left = Mapper.GetCoordinateX(coverRect.Left);
//                var right = Mapper.GetCoordinateX(coverRect.Right);
//                var top = Mapper.GetCoordinateY(coverRect.Top);
//                var bottom = Mapper.GetCoordinateY(coverRect.Bottom);
//                SetMapperRange(left, right, top, bottom);
//            }

//            //此处必须清零coverRect，原因是：当鼠标只做点击不拖动时，coverRect会保留上次放缩时计算的CoverRect值，因此会通过IsRectBigEnough的校验
//            coverRect = new Rectangle(0, 0, 0, 0);
//            return;
//        }

//        private void SetMapperRange(double left, double right, double top, double bottom)
//        {
//            if (!Animation)
//                Mapper.SetCoordinateArea(left, right, top, bottom);
//            else
//            {
//                Area targetArea = new Area(left, right, top, bottom);
//                ZoomAnimation zoomAnimation = new ZoomAnimation(targetArea, Mapper);
//                zoomAnimation.StartZoom();
//            }
//        }

//        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
//        {
//            if (!mouseDown)
//                return;
//            mouseCurrentPos = e.Location;
//            coverRect = SelectStrategy.CalRect(mouseDownPos, mouseCurrentPos);
//            UpdateView();
//        }

//        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
//        {
//            if (!IsOn)
//                return;
//            mouseDown = true;
//            mouseDownPos = e.Location;
//            mouseCurrentPos = e.Location;
//        }

//        public void Reset() => SetMapperRange(ReferenceSystem.Left, ReferenceSystem.Right, ReferenceSystem.Top, ReferenceSystem.Bottom);

//        protected override void DrawElement(RenderTarget rt)
//        {
//            if (!mouseDown)
//                return;
//            SelectStrategy.DrawZoomView(coverRect.ToRectF(), rt, fillBrush, frameBrush, 2);
//        }

//        public void SetStrategy(SelectStrategy s)
//        {
//            lock (Locker)
//            {
//                SelectStrategy = s;
//                s.SetZoomController(this);
//            }
//        }

//        public void On() => isOn = true;

//        public void Off() => isOn = false;
//    }
//}
