using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace Utilities.Display
{
    public abstract class DynamicElement<T> : GraphicElement
    {
        public override void Draw(RenderTarget rt)
        {
            lock(Locker)
            {
                base.Draw(rt);
                DoDraw(rt);
            }
        }

        protected abstract void DoDraw(RenderTarget rt);

        public virtual void Update(T t)
        {
            lock(Locker)
            {
                DoUpdate(t);
                Changed = true;
            }
        }

        protected abstract void DoUpdate(T t);
    }
}
