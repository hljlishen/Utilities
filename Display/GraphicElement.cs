using System;
using System.Windows.Forms;
using Utilities.Mapper;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace Utilities.Display
{
    public abstract class GraphicElement : IDisposable
    {
        protected bool Changed = true;
        protected Displayer displayer;
        protected readonly object Locker = new object();
        protected bool firstTimeDraw = true;
        public void Draw(RenderTarget rt)
        {
            if(firstTimeDraw)
            {
                firstTimeDraw = false;
                InitializeComponents(rt);
            }
            DrawElement(rt);
            Changed = false;
        }
        protected abstract void DrawElement(RenderTarget rt);
        protected virtual void InitializeComponents(RenderTarget rt) { }
        public virtual bool HasChanged()=> Changed;
        public virtual void SetDisplayer(Displayer d) => displayer = d;
        protected Panel Panel => displayer.Panel;
        protected IScreenToCoordinateMapper Mapper => displayer?.Mapper;
        protected Background Background => displayer.Background;
        public virtual void Dispose() { }
    }
}
