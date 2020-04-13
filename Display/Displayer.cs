using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities.Mapper;
using Utilities.Tools;

namespace Utilities.Display
{
    public class Displayer : IDisposable
    {
        public int UpdateInterval { get; set; }
        public bool Redraw { get; set; }
        public Panel Panel { get; set; }
        public IScreenToCoordinateMapper Mapper { get; }
        private RenderTarget rt;
        public D2DFactory Factory { get; protected set; }

        public ReferenceSystem ReferenceSystem;
        private bool Disposed;

        public LayerManager Elements { get; protected set; }
        public readonly object Locker = new object();

        public Displayer(Panel p, IScreenToCoordinateMapper mapper, ReferenceSystem referenceSystem)
        {
            Panel = p;
            #region Mapper
            Mapper = mapper;
            referenceSystem.SetMapper(mapper);
            ReferenceSystem = referenceSystem;
            mapper.SetScreenArea(0, p.Size.Width, 0, p.Size.Height);
            mapper.MapperStateChanged += Mapper_MapperStateChanged;
            #endregion

            #region Background
            Elements = new LayerManager();
            Elements.SetDisplayer(this);
            #endregion

            Panel.SizeChanged += Pb_SizeChanged;
            Factory = D2DFactory.CreateFactory(D2DFactoryType.Multithreaded);   //创建工厂

            StartDrawing();

            rt.Transform = Matrix3x2F.Scale(rt.Size.Width / Panel.Width, rt.Size.Height / Panel.Height);
            Redraw = true;
        }

        private void StartDrawing()
        {
            InitD2d();
            Task.Run(Draw);
        }

        private void InitD2d()
        {
            rt?.Dispose();
            var rtps = new RenderTargetProperties();
            var hrtp = new HwndRenderTargetProperties(Panel.Handle, new SizeU((uint)Panel.Width, (uint)Panel.Height), PresentOptions.None);
            rt = Factory.CreateHwndRenderTarget(rtps, hrtp);
            rt.AntiAliasMode = AntiAliasMode.Aliased;
            rt.TextAntiAliasMode = TextAntiAliasMode.Aliased;
        }
        private void Mapper_MapperStateChanged(IScreenToCoordinateMapper obj) => Redraw = true;

        private void Pb_SizeChanged(object sender, EventArgs e)
        {
            lock(Locker)
            {
                Mapper.SetScreenArea(0, Panel.Width, 0, Panel.Height);
                (rt as HwndRenderTarget).Resize(new SizeU((uint)Panel.Width, (uint)Panel.Height));
                rt.Transform = Matrix3x2F.Scale(rt.Size.Width / Panel.Width, rt.Size.Height / Panel.Height);
                //rt.Transform = Matrix3x2F.Rotation((float)Functions.DegreeToRadian(90), new Point2F(438, 412.5f));
                //Redraw = true;
            }
        }

        private void Draw()
        {
            while (true)
            {
                lock (Locker)
                {
                    if (Disposed)
                        return;
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
                Thread.Sleep(UpdateInterval);
            }
        }

        public void Dispose()
        {
            rt.Dispose();
            Disposed = true;
        }
    }
}
