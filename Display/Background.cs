using System.Drawing;
using System.Windows.Forms;
using Utilities.Mapper;

namespace Utilities.Display
{
    public class BackgroundModel
    {
        public double XLeft;
        public double XRight;
        public double YTop;
        public double YBottom;

        public BackgroundModel(double xLeft, double xRight, double yTop, double yBottom)
        {
            XLeft = xLeft;
            XRight = xRight;
            YTop = yTop;
            YBottom = yBottom;
        }

        public BackgroundModel(BackgroundModel m) : this(m.XLeft, m.XRight, m.YTop, m.YBottom)
        {

        }
    }
    public class Background : DynamicElement<BackgroundModel>
    {
        public double XLeft { get; set; }
        public double XRight { get; set; }
        public double YTop { get; set; }
        public double YBottom { get; set; }

        public BackgroundModel Model {get; protected set;}

        public Color BackgroundColor { get; set; } = Color.Black;
        protected override void DoDraw(Graphics graphics)
        {
            SetMapperCoordinate(Mapper);
            graphics.Clear(BackgroundColor);
        }

        public override void SetDisplayer(Displayer d)
        {
            d.PictureBox.SizeChanged += PictureBox_SizeChanged;
            base.SetDisplayer(d);
        }

        private void PictureBox_SizeChanged(object sender, System.EventArgs e)
        {
            Mapper.SetScreenArea(0, PictureBox.Size.Width, 0, PictureBox.Size.Height);
            Changed = true;
        }

        protected virtual void SetMapperCoordinate(IScreenToCoordinateMapper mapper)
        {
            if (mapper.CoordinateLeft != XLeft || mapper.CoordinateRight != XRight)
                mapper.SetCoordinateXRange(XLeft, XRight);
            if (mapper.CoordinateTop != YTop || mapper.CoordinateBottom != YBottom)
                mapper.SetCoordinateYRange(YTop, YBottom);
        }

        protected override void DoUpdate(BackgroundModel t) => Model = new BackgroundModel(t);
    }
}
