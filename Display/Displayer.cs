using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities.Mapper;

namespace Utilities.Display
{
    public class Displayer : IDisposable
    {
        public Panel PictureBox;
        public IScreenToCoordinateMapper Mapper { get; }
        private System.Windows.Forms.Timer updateTimer;
        private RenderTarget rt;
        public D2DFactory Factory { get; private set; }
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

        public Displayer(Panel pb, AbstractDisplayerFactory factory) : this(pb, factory.GetMapper(), factory.GetBackground())
        {

        }

        public Displayer(Panel pb, IScreenToCoordinateMapper mapper, Background background)
        {
            PictureBox = pb;
            #region Mapper
            Mapper = mapper;
            mapper.SetScreenArea(0, pb.Size.Width, 0, pb.Size.Height);
            mapper.SetCoordinateXRange(background.Model.XLeft, background.Model.XRight);
            mapper.SetCoordinateYRange(background.Model.YTop, background.Model.YBottom);
            mapper.MapperStateChanged += Mapper_MapperStateChanged;
            #endregion

            #region Background
            Elements = new LayeredElement();
            Elements.SetDisplayer(this);
            background.SetDisplayer(this);
            Background = background;
            Elements.AddElement(0, background);
            #endregion

            pb.SizeChanged += Pb_SizeChanged;
            Factory = D2DFactory.CreateFactory(D2DFactoryType.Multithreaded);   //创建工厂
            var rtps = new RenderTargetProperties();
            var hrtp = new HwndRenderTargetProperties(pb.Handle, new SizeU((uint)pb.Width, (uint)pb.Height), PresentOptions.None);
            rt = Factory.CreateHwndRenderTarget(rtps, hrtp);

            updateTimer = new System.Windows.Forms.Timer
            {
                Interval = 30
            };
            updateTimer.Tick += UpdateTimer_Tick;
            //updateTimer.Start();

            Task.Run(Draw);
        }

        private void Mapper_MapperStateChanged(IScreenToCoordinateMapper obj) => Redraw = true;

        private void Pb_SizeChanged(object sender, EventArgs e)
        {
            //Mapper.SetScreenArea(0, PictureBox.Width, 0, PictureBox.Height);
            //lock(Locker)
            //{
            //    rt?.Dispose();
            //    var rtps = new RenderTargetProperties();
            //    var hrtp = new HwndRenderTargetProperties(PictureBox.Handle, new SizeU((uint)PictureBox.Width, (uint)PictureBox.Height), PresentOptions.None);
            //    rt = Factory.CreateHwndRenderTarget(rtps, hrtp);
            //}
        }

        private void Draw()
        {
            while (true)
            {
                UpdateTimer_Tick(null, null);
                Thread.Sleep(30);
            }
        }

        private void UpdateTimer_Tick(object sender, System.EventArgs e)
        {
            lock (Locker)
            {
                rt.BeginDraw();
                rt.Clear();
                if (Redraw)
                {
                    Elements?.Draw(rt);
                    Redraw = false;
                }
                else
                {
                    Elements.DrawChangedLayers(rt);
                }

                rt.EndDraw();
            }
        }
        public static void InitializeGraphics(Graphics graphics)
        {
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
        }

        public void Dispose()
        {
            rt.Dispose();
        }
    }
}
