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
        public int UpdateInterval { get; set; }
        public bool Redraw { get; set; }
        public Panel Panel { get; private set; }
        public event Action BeforeRebindTarget;
        public event Action AfterRebindTarget;

        public IScreenToCoordinateMapper Mapper { get; }
        private RenderTarget rt;
        public D2DFactory Factory { get; protected set; }

        public ReferenceSystem ReferenceSystem;

        public LayerManager Elements { get; protected set; }
        public readonly object Locker = new object();

        private CancellationTokenSource tokenSource;
        private CancellationToken token;

        public Displayer(Panel p, IScreenToCoordinateMapper mapper, ReferenceSystem referenceSystem)
        {
            Panel = p;
            Panel.SizeChanged += Pb_SizeChanged;

            Mapper = mapper;
            referenceSystem.SetMapper(mapper);
            ReferenceSystem = referenceSystem;
            mapper.SetScreenArea(0, p.Size.Width, 0, p.Size.Height);
            mapper.MapperStateChanged += Mapper_MapperStateChanged;

            Elements = new LayerManager();
            Elements.SetDisplayer(this);

            InitializeDisplayerState();
        }

        public void Start()
        {
            tokenSource?.Dispose();
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;
            Redraw = true;
            Task.Run(Draw);
        }

        private void InitializeDisplayerState()
        {
            var rtps = new RenderTargetProperties();
            var hrtp = new HwndRenderTargetProperties(Panel.Handle, new SizeU((uint)Panel.Width, (uint)Panel.Height), PresentOptions.None);

            Factory = D2DFactory.CreateFactory(D2DFactoryType.Multithreaded);   //创建工厂
            rt = Factory.CreateHwndRenderTarget(rtps, hrtp);
            rt.AntiAliasMode = AntiAliasMode.Aliased;
            rt.TextAntiAliasMode = TextAntiAliasMode.Aliased;
            (rt as HwndRenderTarget).Resize(new SizeU((uint)Panel.Width, (uint)Panel.Height));
            rt.Transform = Matrix3x2F.Scale(rt.Size.Width / Panel.Width, rt.Size.Height / Panel.Height);
        }

        public void Stop()
        {
            tokenSource.Cancel();
            Thread.Sleep(10);
            Color c = Panel.BackColor;
            Panel.BackColor = Color.Black;   //需要先置为黑色，给backcolor赋值本来的颜色不会产生任何操作
            Panel.BackColor = c;  //backcolor置为本来的颜色
        }

        public void SetPanel(Panel p)
        {
            Stop();
            ChangePanel(p);
            DisposeRenderTarget();
            InitializeDisplayerState();
        }

        private void ChangePanel(Panel p)
        {
            Panel.SizeChanged -= Pb_SizeChanged; //接触消息绑定
            BeforeRebindTarget?.Invoke();
            Panel = p;
            Panel.SizeChanged += Pb_SizeChanged;//消息重新绑定
            Mapper.SetScreenArea(0, p.Size.Width, 0, p.Size.Height);
            AfterRebindTarget?.Invoke();
        }

        private void DisposeRenderTarget()
        {
            Factory.Dispose();
            Factory = null;
            rt.Dispose();
            rt = null;
        }

        private void Mapper_MapperStateChanged(IScreenToCoordinateMapper obj) => Redraw = true;

        private void Pb_SizeChanged(object sender, EventArgs e)
        {
            lock (Locker)
            {
                if (Panel.Width < 10 && Panel.Height < 10)  //卫语句，窗口最小化时会触发sizechanged事件，此时width和height都是0，会触发ValueInterval的范围过小异常
                    return;
                Mapper.SetScreenArea(0, Panel.Width, 0, Panel.Height);
                (rt as HwndRenderTarget).Resize(new SizeU((uint)Panel.Width, (uint)Panel.Height));
                rt.Transform = Matrix3x2F.Scale(rt.Size.Width / Panel.Width, rt.Size.Height / Panel.Height);
                Redraw = true;
            }
        }

        private void Draw()
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                lock (Locker)
                {
                    //if (Disposed)
                    //    return;
                    rt.BeginDraw();
                    rt.Clear();

                    if (Redraw)
                    {
                        Elements.Draw(rt);
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
            //Disposed = true;
        }
    }
}
