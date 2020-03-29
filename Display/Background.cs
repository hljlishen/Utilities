using System.Drawing;
using System.Windows.Forms;
using Utilities.Mapper;

namespace Utilities.Display
{
    public abstract class Background : ThreadSafeElement
    {
        public double XLeft { get; set; }
        public double XRight { get; set; }
        public double YTop { get; set; }
        public double YBottom { get; set; }

        public Color BackgroundColor { get; set; } = Color.Black;
        protected override void DoDraw(Graphics graphics, IScreenToCoordinateMapper mapper)
        {
            SetMapperCoordinate(mapper);
            graphics.Clear(BackgroundColor);
        }

        protected override void ProcessMouseDown(object sender, MouseEventArgs e, Displayer displayer) { }
        protected override void ProcessMouseMove(object sender, MouseEventArgs e, Displayer displayer) { }
        protected override void ProcessMouseUp(object sender, MouseEventArgs e, Displayer displayer) { }
        public void SizeChanged(Size s, IScreenToCoordinateMapper mapper) => mapper.SetScreenArea(0, s.Width, 0, s.Height);
        protected virtual void SetMapperCoordinate(IScreenToCoordinateMapper mapper)
        {
            if (mapper.CoordinateLeft != XLeft || mapper.CoordinateRight != XRight)
                mapper.SetCoordinateXRange(XLeft, XRight);
            if (mapper.CoordinateTop != YTop || mapper.CoordinateBottom != YBottom)
                mapper.SetCoordinateYRange(YTop, YBottom);
        }
    }
}
