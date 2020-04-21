using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace Utilities.Display
{
    public abstract class DynamicElement<T> : GraphicElement where T : new()
    {
        protected T Model = new T();
        protected override void DrawElement(RenderTarget rt)
        {
            lock (Locker)
            {
                DrawDynamicElement(rt);
            }
        }

        protected abstract void DrawDynamicElement(RenderTarget rt);

        public virtual void Update(T t)
        {
            lock (Locker)
            {
                DoUpdate(t);
                RefreshObjects();
                UpdateView();
            }
        }

        protected virtual void DoUpdate(T t)
        {
            Model = t;
        }
    }
}
