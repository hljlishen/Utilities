using System.Drawing;
using System.Windows.Forms;
using Utilities.Mapper;

namespace Utilities.Display
{
    public class Displayer
    {
        public PictureBox PictureBox;
        public IScreenToCoordinateMapper Mapper { get; }
        private Timer updateTimer;
        private Bitmap canvas;
        public Background Background { get; protected set; }
        public LayeredElement Elements { get; private set; }
        private readonly object Locker = new object();
        public int UpdateInterval 
        {
            get => updateTimer.Interval;
            set
            {
                updateTimer.Stop();
                updateTimer.Interval = value;
                updateTimer.Start();
            }
        }

        public bool Redraw { get; set; }

        public Displayer(PictureBox pb, AbstractDisplayerFactory factory) : this(pb, factory.GetMapper(), factory.GetBackground())
        {

        }

        public Displayer(PictureBox pb, IScreenToCoordinateMapper mapper, Background background)
        {
            PictureBox = pb;
            Mapper = mapper;
            mapper.SetScreenArea(0, pb.Size.Width, 0, pb.Size.Height);
            mapper.SetCoordinateXRange(background.Model.XLeft, background.Model.XRight);
            mapper.SetCoordinateYRange(background.Model.YTop, background.Model.YBottom);
            mapper.MapperStateChanged += Mapper_MapperStateChanged;
            Elements = new LayeredElement();
            Elements.SetDisplayer(this);
            background.SetDisplayer(this);
            Background = background;
            Elements.AddElement(0, background);

            canvas = new Bitmap(pb.Width, pb.Height);
            pb.Image = canvas;
            pb.SizeChanged += Pb_SizeChanged;

            updateTimer = new Timer
            {
                Interval = 30
            };
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();
        }

        private void Mapper_MapperStateChanged(IScreenToCoordinateMapper obj) => Redraw = true;

        private void Pb_SizeChanged(object sender, System.EventArgs e)
        {
            lock (Locker)
            {
                var bmp = PictureBox.Image;
                canvas = new Bitmap(PictureBox.Width, PictureBox.Height);
                PictureBox.Image = canvas;
                bmp.Dispose();
            }
        }

        private void UpdateTimer_Tick(object sender, System.EventArgs e)
        {
            lock(Locker)
            {
                using (Graphics graphics = Graphics.FromImage(canvas))
                {
                    InitializeGraphics(graphics);
                    graphics.Clear(Color.Black);
                    if (Redraw)
                    {
                        Elements?.Draw(graphics);
                        Redraw = false;
                    }
                    else
                    {
                        Elements.DrawChangedLayers(graphics);
                    }
                }

                PictureBox.Refresh();
            }
        }
        public static void InitializeGraphics(Graphics graphics)
        {
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
        }
    }
}
