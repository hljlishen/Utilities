using System;
using System.Windows.Forms;
using Utilities.Mapper;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace Utilities.Display
{
    public abstract class GraphicElement : IDisposable
    {
        public int LayerId { get; set; }
        private bool Changed = true;
        protected Displayer displayer;
        protected readonly object Locker = new object();
        private bool firstTimeDraw = true;
        public ReferenceSystem ReferenceSystem => displayer.ReferenceSystem;
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
        public virtual void UpdateGraphic() => Changed = true;

        /// <summary>
        /// 框架负责调用次函数，用户不要主动调用
        /// </summary>
        /// <param name="d">显示器</param>
        public virtual void SetDisplayer(Displayer d) => displayer = d;
        public Panel Panel => displayer.Panel;
        public virtual IScreenToCoordinateMapper Mapper => displayer.Mapper;
        public virtual void Dispose() { }
    }
}
