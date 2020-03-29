using System.Drawing;
using System.Windows.Forms;
using Utilities.Mapper;

namespace Utilities.Display
{
    public class Displayer
    {
        private PictureBox pb;
        public IScreenToCoordinateMapper Mapper { get; }
        private Timer updateTimer;
        public Background Background { get; set; }
        private Bitmap canvas;
        public LayeredElement Elements { get; set; }
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

        public Displayer(PictureBox pb, IScreenToCoordinateMapper mapper)
        {
            this.pb = pb;
            this.Mapper = mapper;
            mapper.SetScreenArea(0, pb.Size.Width, 0, pb.Size.Height);

            canvas = new Bitmap(pb.Width, pb.Height);
            pb.Image = canvas;

            pb.MouseDown += Pb_MouseDown;
            pb.MouseMove += Pb_MouseMove;
            pb.MouseUp += Pb_MouseUp;
            pb.SizeChanged += Pb_SizeChanged;

            updateTimer = new Timer
            {
                Interval = 30
            };
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();
        }

        private void Pb_SizeChanged(object sender, System.EventArgs e)
        {
            canvas.Dispose();
            canvas = new Bitmap(pb.Size.Width, pb.Size.Height);
            pb.Image = canvas;
            Background.SizeChanged(pb.Size, Mapper);
        }

        private void Pb_MouseUp(object sender, MouseEventArgs e) => Elements?.MouseUp(sender, e, this);

        private void Pb_MouseMove(object sender, MouseEventArgs e) => Elements?.MouseMove(sender, e, this);

        private void Pb_MouseDown(object sender, MouseEventArgs e) => Elements?.MouseDown(sender, e, this);

        private void UpdateTimer_Tick(object sender, System.EventArgs e)
        {
            using (Graphics graphics = Graphics.FromImage(canvas))
            {
                InitializeGraphics(graphics);
                Background?.Draw(graphics, Mapper);
                Elements?.Draw(graphics, Mapper);
            }

            pb.Refresh();
        }
        private static void InitializeGraphics(Graphics graphics)
        {
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
        }
    }
}
